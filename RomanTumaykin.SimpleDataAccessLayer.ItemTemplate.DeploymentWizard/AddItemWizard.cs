using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RomanTumaykin.SimpleDataAccessLayer.ItemTemplate.DeploymentWizard
{
    public class AddItemWizard : IWizard
    {
        private EnvDTE.ProjectItem dal, template;
        bool canAdd = true;

        internal static Project GetActiveProject(DTE2 dte)
        {
            Project project = null;
            try
            {
                Array activeSolutionProjects = (Array)dte.ActiveSolutionProjects;
                if (activeSolutionProjects.Length > 0)
                {
                    project = (Project)activeSolutionProjects.GetValue(0);
                }
            }
            catch (COMException)
            {
            }
            if (project == null)
            {
                if (((dte.ActiveDocument != null) && (dte.ActiveDocument.ProjectItem != null)) && (dte.ActiveDocument.ProjectItem.ContainingProject != null))
                {
                    return dte.ActiveDocument.ProjectItem.ContainingProject;
                }
                if (dte.Solution.Projects.Count > 0)
                {
                    project = dte.Solution.Projects.Item(1);
                }
            }
            return project;
        }

        // Taken from Entity Framework implementation. Since you have to have an item selected when you 
        // either right click or choose a menu, then it is possible to find the path where all items will be generated
        private static string GetFolderNameForNewItems(DTE2 dte)
        {
            Project _activeProject = GetActiveProject(dte);

            SelectedItem item = dte.SelectedItems.Item(1);
            string fullName = null;
            if (item.Project != null)
            {
                fullName = GetProjectRoot(item.Project).FullName;
            }
            else if (item.ProjectItem != null)
            {
                DirectoryInfo parent = new DirectoryInfo(item.ProjectItem.get_FileNames(1));
                while ((parent.Attributes & FileAttributes.Directory) != FileAttributes.Directory)
                {
                    parent = parent.Parent;
                }
                fullName = parent.FullName;
            }
            else
            {
                fullName = GetProjectRoot(_activeProject).FullName;
            }

            return fullName;
        }

        internal static DirectoryInfo GetProjectRoot(Project project)
        {
            DirectoryInfo info = null;

            string str4 = project.Properties.Item("FullPath").Value as string;
            if (!string.IsNullOrEmpty(str4))
            {
                info = new DirectoryInfo(str4);
            }

            if (info == null)
            {
                info = new DirectoryInfo(@".\");
            }
            return info;
        }

        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {

        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
            try
            {
                if (projectItem.Name.Substring(projectItem.Name.Length - 3, 3) == "dal")
                {
                    this.dal = projectItem;
                }
                else
                {
                    this.template = projectItem;
                }
            }
            catch (Exception e)
            {
                canAdd = false;
            }
        }

        public void RunFinished()
        {
            if (canAdd)
                this.dal.ProjectItems.AddFromFile(this.template.FileNames[1]);
        }


        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            DTE2 _dte = (DTE2)automationObject;

            String _folderNameForNewItems = GetFolderNameForNewItems(_dte);
            String _mainFileName = _folderNameForNewItems + "\\" + replacementsDictionary["$safeitemname$"];

            int _index = 0;
            bool _keepNameSearching = false;
            string _ttNewPath = null;
            do
            {
                // if this item exists in solution - keep searching
                while (_dte.Solution.FindProjectItem(_mainFileName + (_index == 0 ? "" : _index.ToString()) + ".tt") != null)
                {
                    _index++;
                }
                _ttNewPath = _mainFileName + (_index == 0 ? "" : _index.ToString()) + ".tt";
                if (File.Exists(_ttNewPath))
                {
                    DialogResult _result = MessageBox.Show(String.Format("File {0} exists, but it is not a part of the project. Overwrite?", _ttNewPath), "File conflict", MessageBoxButtons.YesNoCancel);
                    if (_result == DialogResult.Yes)
                    {
                        File.Delete(_ttNewPath);
                        _keepNameSearching = false;
                    }
                    else if (_result == DialogResult.No)
                    {
                        //increase index and keep searching
                        _index++;
                        _keepNameSearching = true;
                    }
                    else if (_result == DialogResult.Cancel)
                    {
                        _keepNameSearching = false;
                        this.canAdd = false;
                        break;
                    }
                }
                else
                {
                    _keepNameSearching = false;
                }

            } while (_keepNameSearching);

            replacementsDictionary.Add("$fileinputname_randomized$", Path.GetFileNameWithoutExtension(_ttNewPath));
            
            // make sure that the "D:\Users\rtumaykin\AppData\Local\Microsoft\VisualStudio\11.0Exp\VTC\86e5fb2ca5ba64738f07b472c28dca98\~IC\ItemTemplates\Data\CSharp\1033\DataAccessLayerItemTemplate.zip\DataAccessLayerItemTemplate.vstemplate"
            // path is always passed as the first parameter to the customParams (http://msdn.microsoft.com/en-us/library/ms247063(v=vs.100).aspx)
            // and if this is always true, then I can read the file content into the string, then at the end save straight to the disk
            // this will be used for the temporary file name for tt template until it is renamed to something else.
            
            
            
            
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            // don't add the tt file
            return canAdd; // filePath.Substring(filePath.Length - 3) == ".tt" ? false : true;
        }
    }
}
