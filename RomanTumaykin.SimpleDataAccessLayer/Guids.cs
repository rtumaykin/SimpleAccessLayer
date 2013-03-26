// Guids.cs
// MUST match guids.h
using System;

namespace RomanTumaykin.SimpleDataAccessLayer
{
    static class GuidList
    {
        public const string guidSimpleDataAccessLayerConfigFileEditorPkgString = "17a55fbc-34a2-45ed-b9f6-8ce882621048";
        public const string guidSimpleDataAccessLayerConfigFileEditorCmdSetString = "dda75a2b-4df6-4c6d-b1b4-7cc1eeced1fe";
        public const string guidSimpleDataAccessLayerConfigFileEditorEditorFactoryString = "65a05557-3135-4de7-be84-99a315e09f2d";

        public static readonly Guid guidSimpleDataAccessLayerConfigFileEditorCmdSet = new Guid(guidSimpleDataAccessLayerConfigFileEditorCmdSetString);
        public static readonly Guid guidSimpleDataAccessLayerConfigFileEditorEditorFactory = new Guid(guidSimpleDataAccessLayerConfigFileEditorEditorFactoryString);
    };
}