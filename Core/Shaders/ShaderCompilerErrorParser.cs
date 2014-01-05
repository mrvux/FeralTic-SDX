using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11
{
    public static class ShaderCompilerErrorParser
    {
        private static void ParseLine(this CompilerResults result, string line, string locaPath, string exePath)
        {
            var eItems = line.Split(new string[1] { ": " }, StringSplitOptions.None);

            string filePath = locaPath;
            string eCoords = string.Empty;
            int eLine = 0;
            int eChar = 0;
            string eNumber = string.Empty;
            string eText = string.Empty;
            bool isWarning = false;

            if (eItems.Length == 2)
            {
                eLine = 0;
                eChar = 0;
                isWarning = eItems[0].Contains("warning");
                eNumber = eItems[0].Replace("wanring", "").Trim();
                eText = eItems[1];
            }
            else
            {
                //extract line/char substring
                int start = eItems[0].LastIndexOf('(');
                int end = eItems[0].LastIndexOf(')');

                if (start > 0)
                {
                    string relativePath = eItems[0].Substring(0, start);

                    //if this is a path to an include..
                    if (relativePath != Path.Combine(FHDEHost.ExePath, "memory"))
                    {
                        // we need to guess here. shader compiler outputs relative paths.
                        // we don't know if the include was "local" or <global>

                        filePath = Path.Combine(Path.GetDirectoryName(project.LocalPath), relativePath);
                        if (!File.Exists(filePath))
                        {
                            string fileName = Path.GetFileName(relativePath);

                            foreach (var reference in project.References)
                            {
                                var referenceFileName = Path.GetFileName((reference as FXReference).ReferencedDocument.LocalPath);
                                if (referenceFileName.ToLower() == fileName.ToLower())
                                {
                                    filePath = reference.AssemblyLocation;
                                }
                            }
                        }
                    }
                }

                if (start > -1 && end > 0)
                {
                    eCoords = eItems[0].Substring(start + 1, end - start - 1);
                    var eLineChar = eCoords.Split(new char[1] { ',' });
                    eLine = Convert.ToInt32(eLineChar[0]);

                    if (eLineChar[1].Contains("-"))
                    {
                        string[] spl = eLineChar[1].Split("-".ToCharArray());
                        eChar = Convert.ToInt32(spl[0]);
                    }
                    else
                    {
                        eChar = Convert.ToInt32(eLineChar[1]);

                    }

                    if (eItems[1].StartsWith("warning"))
                    {
                        isWarning = true;
                        eNumber = eItems[1].Substring(8, 5);
                    }
                    else
                        eNumber = eItems[1].Substring(6, 5);

                    if (eItems.Length == 2)
                    {
                        isWarning = false;
                        eNumber = "-1";
                        eText = eItems[1];
                    }
                    else
                    {
                        eText = eItems[2];
                        if (eItems.Length > 3)
                            eText += ": " + eItems[3];
                    }
                }
                else
                {
                    eText = line;
                }
            }

            var error = new CompilerError(filePath, eLine, eChar, eNumber, eText);
            error.IsWarning = isWarning;
            result.Errors.Add(error);
        }

        public static CompilerResults ParseCompilerResult(string ErrorMessage, string localPath)
        {
            CompilerResults compilerResults = new CompilerResults(null);

            var errorlines = ErrorMessage.Split(new char[1] { '\n' });

            foreach (var line in errorlines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    compilerResults.ParseLine(line,localPath);
                }
            }


            return compilerResults;
        }


    }
}
