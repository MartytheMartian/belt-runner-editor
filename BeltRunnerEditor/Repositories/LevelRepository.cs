using System;
using System.IO;
using System.Xml;
using Microsoft.Win32;

using BeltRunnerEditor.Entities;
using BeltRunnerEditor.Interfaces;

namespace BeltRunnerEditor.Repositories
{
    /// <summary>
    /// Reads and writes levels via file system
    /// </summary>
    public class LevelRepository : ILevelRepository
    {
        /// <summary>
        /// Reads in a new level
        /// </summary>
        /// <returns>New level</returns>
        public Level Read()
        {
            // Return value
            Level level = null;

            // Setup a dialog for a level
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".xml";
            dialog.Filter = "XML Document (.xml)|*.xml";

            // Open the dialog
            bool? success = dialog.ShowDialog();

            // Default file name if none was selected
            string file = success.HasValue && success.Value ? dialog.FileName : null;

            // Quick out for no file selected
            if (String.IsNullOrWhiteSpace(file))
            {
                return level;
            }

            // Parse the file as XML
            XmlDocument document = new XmlDocument();
            document.Load(file);

            // Populate the level
            level = new Level(file, document);

            return level;
        }

        /// <summary>
        /// Saves a level
        /// </summary>
        /// <param name="level">Level to save</param>
        public void Save(Level level)
        {
            // Save as a new file if there is no path
            if (String.IsNullOrWhiteSpace(level.FileName))
            {
                SaveAs(level);
                return;
            }

            // Generate level XML
            string xml = level.ToXml();

            // Delete the file if it already exists
            if (File.Exists(level.FileName))
            {
                File.Delete(level.FileName);
            }

            // Create a new file
            File.WriteAllText(level.FileName, xml);
        }

        /// <summary>
        /// Save a level as a new file
        /// </summary>
        /// <param name="level">Level to save</param>
        public void SaveAs(Level level)
        {
            // Generate level XML
            string xml = level.ToXml();

            // Save dialog for a level
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".xml";
            dialog.Filter = "XML Document (.xml)|*.xml";

            // Open the dialog
            bool? success = dialog.ShowDialog();
            if (!success.HasValue || !success.Value)
            {
                return;
            }

            // Delete the file if it already exists
            if (File.Exists(dialog.FileName))
            {
                File.Delete(dialog.FileName);
            }

            // Create a new file
            File.WriteAllText(dialog.FileName, xml);
        }
    }
}
