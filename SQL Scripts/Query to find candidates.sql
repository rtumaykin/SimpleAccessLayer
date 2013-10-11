WITH Candidates AS (
	-- this gets the first seed set of rows
	-- only rows with single column primary keys are included
	SELECT 
		[i].[object_id] AS [TableId],
		[i].[index_id] AS [PK_IndexId],
		NULL AS [Parent_TableId],
		NULL AS [Parent_PK_IndexId],
		0 AS [Level]
	FROM [sys].[indexes] i
		INNER JOIN [sys].[index_columns] ic
			ON	[ic].[object_id] = [i].[object_id]
				AND [ic].[index_id] = [i].[index_id]
		CROSS APPLY(
			SELECT TOP 1 _c.[object_id]
			FROM sys.[columns] _c
				LEFT OUTER JOIN [sys].[index_columns] _ic
					ON	[_ic].[object_id] = [_c].[object_id]
						AND [_ic].[column_id] = [_c].[column_id]
						AND [_ic].[index_id] = i.[index_id]
			WHERE 
				_c.[object_id] = i.[object_id]
				AND _c.[system_type_id] IN (167, 175, 231, 239)
--				AND _ic.[column_id] IS NULL
				-- at this point I don't need to make sure that the column is not a part of PK 
				-- since in the single column pk table it is allowed to have Key and Value the same
		) c
		INNER JOIN sys.[objects] o
			ON	[o].[object_id] = [i].[object_id]
		INNER JOIN [sys].[columns] col	
			ON	[col].[object_id] = [ic].[object_id]
				AND [col].[column_id] = [ic].[column_id]
	WHERE 
		[i].[is_primary_key] = 1
		AND [i].[is_disabled] = 0
		AND [o].[type] IN ('U', 'V')
		-- text or numeric
		AND [col].[system_type_id] IN (35, 36, 48, 52, 56, 59, 60, 62, 99, 104, 106, 108, 122, 127, 167, 175, 231, 239, 241, 231)
	GROUP BY 
		[i].[object_id],
		[i].[index_id]
	HAVING COUNT(*) = 1
	UNION ALL
	SELECT  [TableId],
			[PK_IndexId],
			[Parent_TableId],
			[Parent_PK_IndexId],
			[Level] + 1 AS [Level]
	FROM (
		SELECT 
			OBJECT_NAME(c.[object_id]) AS [TableName],
			OBJECT_NAME(p.[TableId]) AS [ParentTableName],
			[c].[object_id] AS [TableId],
			i.[index_id] AS [PK_IndexId],
			[p].[TableId] AS [Parent_TableId],
			[p].[PK_IndexId] AS [Parent_PK_IndexId],
			COUNT(*) OVER (PARTITION BY c.[object_id]) AS RowsPerIndex,
			MAX(p.[Level]) OVER (PARTITION BY 1) AS [Level]
		FROM (
				SELECT 
					c.[TableId],
					c.[PK_IndexId],	
					c.[Level],
					CONVERT(varbinary(MAX), 
						(
							SELECT CONVERT(varchar(max), CONVERT(binary(4), [column_id]), 2)
							FROM [sys].[index_columns] 
							WHERE 
								[object_id] = [c].[TableId]
								AND [index_id] = [c].[PK_IndexId]
							ORDER BY [column_id] ASC
							FOR XML PATH('')
						), 2
					) AS PK_Footprint
				FROM Candidates c
			) p
			INNER JOIN (
				SELECT  [fk].[parent_object_id] AS [object_id],
						[fk].[object_id] AS [foreign_key_id],
						[fk].[referenced_object_id],
						CONVERT(varbinary(MAX), 
							(
								SELECT CONVERT(varchar(max), CONVERT(binary(4), [parent_column_id]), 2)
								FROM [sys].[foreign_key_columns] 
								WHERE 
									[object_id] = [fk].[object_id]
									AND [parent_object_id] = [fk].[parent_object_id]
									AND [referenced_object_id] = [fk].[referenced_object_id]
								ORDER BY [parent_column_id] ASC
								FOR XML PATH('')
							), 2
						) AS footprint,
						CONVERT(varbinary(MAX), 
							(
								SELECT CONVERT(varchar(max), CONVERT(binary(4), [referenced_column_id]), 2)
								FROM [sys].[foreign_key_columns] 
								WHERE 
									[object_id] = [fk].[object_id]
									AND [parent_object_id] = [fk].[parent_object_id]
									AND [referenced_object_id] = [fk].[referenced_object_id]
								ORDER BY [referenced_column_id] ASC
								FOR XML PATH('')
							), 2
						) AS [referenced_object_footprint]
				FROM [sys].[foreign_keys] fk
			) c
				ON	c.[referenced_object_id] = p.[TableId]
					AND c.[referenced_object_footprint] = p.[PK_Footprint]
			INNER JOIN (
				SELECT
					i.[object_id],
					i.[index_id],	
					CONVERT(varbinary(MAX), 
						(
							SELECT CONVERT(varchar(max), CONVERT(binary(4), [column_id]), 2)
							FROM [sys].[index_columns] 
							WHERE 
								[object_id] = [i].[object_id]
								AND [index_id] = [i].[index_id]
							ORDER BY [column_id] ASC
							FOR XML PATH('')
						), 2
					) AS PK_Footprint
					FROM [sys].[indexes] i
					WHERE
						[i].[is_primary_key] = 1
						AND [i].[is_disabled] = 0
			) i
				ON	i.[object_id] = c.[object_id]
					AND SUBSTRING (i.[PK_Footprint], 1, LEN(i.[PK_Footprint]) - 4) = c.[footprint]
			-- make sure that the rightmost index column is a numeric or text value
			INNER JOIN sys.[columns] col
				ON	[col].[object_id] = [i].[object_id]
					AND col.[column_id] = CONVERT(int, SUBSTRING(i.[PK_Footprint], LEN(i.[PK_Footprint]) - 3, 4)) -- -3 here because position starts with 1
		WHERE col.[system_type_id] IN (35, 36, 48, 52, 56, 59, 60, 62, 99, 104, 106, 108, 122, 127, 167, 175, 231, 239, 241, 231)
	) ch
	WHERE [RowsPerIndex] = 1
)
SELECT  [TableId],
		OBJECT_SCHEMA_NAME([TableId]) AS [SchemaName],
		OBJECT_NAME([TableId]) AS [TableName],
        [Parent_TableId],
        [col].[name] AS Value_ColumnName,
		CONVERT(xml, (
			SELECT 
				_c.[column_id] AS ColumnId,
				_c.[name] AS ColumnName
			FROM [sys].[columns] _c
				LEFT OUTER JOIN [sys].[index_columns] _ic
						ON	_ic.[object_id] = _c.[object_id]
							AND _ic.[column_id] = _c.[column_id]
							AND _ic.[index_id] = c.[PK_IndexId]
			WHERE 
				_c.[object_id] = c.[TableId]
				AND _c.[system_type_id] IN (167, 175, 231, 239)
				AND _ic.[column_id] IS NULL
			FOR XML PATH('Column'), ROOT ('KeyColumns'), ELEMENTS
		)) AS KeyColumnsXml,
		[c].[Level]
FROM (
	SELECT  c.[TableId],
			c.[PK_IndexId],
			c.[Parent_TableId],
			c.[Parent_PK_IndexId],
			c.[Level],
			COUNT(*) OVER (PARTITION BY [TableId]) AS CountPerTable
	FROM [Candidates] c
) c
	CROSS APPLY (
		SELECT TOP 1 [column_id]
		FROM sys.[index_columns]
		WHERE
			[object_id] = c.[TableId]
			AND [index_id] = c.[PK_IndexId]
		ORDER BY [key_ordinal] DESC
	) ic
	INNER JOIN [sys].[columns] col
		ON	[col].[object_id] = c.[TableId]
			AND [col].[column_id] = ic.[column_id]
WHERE [CountPerTable] = 1
ORDER BY c.[Level] ASC;

