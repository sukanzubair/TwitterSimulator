using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using log4net;
using FileReader.Properties;

namespace FileReader
{
    public abstract class BaseFileReader 
    {
        private ILog _logger;

        public BaseFileReader(ILog logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Used to split each line in a file  by the first occurance of the marker
        /// </summary>
        /// <param name="filename">file name of the file to process</param>
        /// <param name="marker">the string to split the line by "delimeter"</param>
        /// <returns>collection of each splits string which represnt each  line in the file split by the "marker"</returns>
        protected IEnumerable<Tuple<string, string>> GetFileSplit(string filename, string marker)
        {
            if (!File.Exists(filename))
            {
                string error = $"File: {Settings.Default.userFileFullName}, does not exists";

                _logger.Error(error);
                throw new FileNotFoundException(error);
            }

            string line = string.Empty;
            List<Tuple<string, string>> splitData = new List<Tuple<string, string>>();
            using (StreamReader file = new StreamReader(filename, Encoding.ASCII))
            {
                _logger.Info($"START Processing file - { filename } ");
                while ((line = file.ReadLine()) != null)
                {
                    int splitPosition = line.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
                    if (splitPosition <= 0)
                    {
                        _logger.Error($"Malformed line detected and will be skipped - { line } ");
                        continue;
                    }

                    string firstPart = line.Substring(0, splitPosition).Trim();
                    if (string.IsNullOrEmpty(firstPart))
                    {
                        _logger.Error($"Malformed line detected, and will be skipped - { line } ");
                        continue;
                    }

                    int start = splitPosition + marker.Length;                    
                    yield return Tuple.Create(firstPart.Trim(), line.Substring(start, line.Length - start));
                }
                _logger.Info($"END Processing file - { filename } ");                
            }

        }
    }
}
