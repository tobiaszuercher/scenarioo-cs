﻿/* scenarioo-api
 * Copyright (C) 2014, scenarioo.org Development Team
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * As a special exception, the copyright holders of this library give you 
 * permission to link this library with independent modules, according 
 * to the GNU General Public License with "Classpath" exception as provided
 * in the LICENSE file that accompanied this code.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Scenarioo.Api.Configuration;

namespace Scenarioo.Api.Util.Xml
{
    using Exception = System.Exception;

    /// <summary>
    /// Writing or reading all documentation entities to XML files and back.
    /// </summary>
    public class ScenarioDocuXMLFileUtil
    {
        public const string SchemaInstanceNamespace = "http://www.w3.org/2001/XMLSchema-instance";

        public const string ScenarioNameSpace = "http://www.scenarioo.org/scenarioo";

        public const string XmlSchema = "http://www.w3.org/2001/XMLSchema";

        public static IList<Task> RunningTasks = new List<Task>();

        public static string XmlKeyIdentifier = "key";

        public static string XmlValueIdentifier = "value";

        public static T UnmarshalXml<T>(string srcFile) where T : class
        {
            if (!File.Exists(srcFile))
            {
                throw new FileNotFoundException(srcFile);
            }

            Lock(
                srcFile,
                f =>
                    {
                        try
                        {
                            var cbuffer = new byte[] { };
                            f.Write(cbuffer, 0, cbuffer.Length);
                        }
                        catch (IOException e)
                        {
                            throw new Exception(string.Format("Could not unmarshall Object from file:{0}", srcFile), e);
                        }
                    });

            using (var fs = new FileStream(srcFile, FileMode.Open, FileAccess.Read, FileShare.None, ScenarioDocuGeneratorConfiguration.AsyncWritingBufferSize, true))
            {
                var desirializedObject = ScenarioDocuXMLUtil.UnmarshalXml<T>(fs);
                fs.Flush();
                fs.Close();

                return desirializedObject;
            }
        }

        /// <summary>
        /// Starts an async task for to marshall an XML in a Thread-Pool
        /// </summary>
        public static void MarshalXml<T>(T entity, string destFile) where T : class
        {
            Lock(
                destFile,
                f =>
                    {
                        try
                        {
                            var cbuffer = new byte[] { };
                            f.Write(cbuffer, 0, cbuffer.Length);
                        }
                        catch (IOException e)
                        {
                            throw new Exception(
                                string.Format(
                                    "Could not marshall Object of type:{0} into file:{1}",
                                    entity.GetType().Name,
                                    destFile),
                                e);
                        }
                    });

            do
            {
                if (RunningTasks.Count <= ScenarioDocuGeneratorConfiguration.MaxConcurrentTasks)
                {
                    var task = new Task(
                        () =>
                            {
                                using (
                                    var fs = new FileStream(
                                        destFile, FileMode.Create, FileAccess.Write, FileShare.None, ScenarioDocuGeneratorConfiguration.MaxConcurrentTasks, true))
                                {
                                    ScenarioDocuXMLUtil.MarshalXml(entity, fs);

                                    fs.Flush();
                                    fs.Close();
                                }
                            });

                    task.Start();

                    RunningTasks.Add(task);
                }

                RemoveFinishedTasks();

                Thread.Sleep(100);
            }
            while (RunningTasks.Count > ScenarioDocuGeneratorConfiguration.MaxConcurrentTasks);
        }

        public static void Lock(string srcPath, Action<FileStream> action)
        {
            var autoResetEvent = new AutoResetEvent(false);

            while (true)
            {
                try
                {
                    using (var file = File.Open(srcPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Write))
                    {
                        action(file);
                        break;
                    }
                }
                catch (IOException)
                {
                    var fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(srcPath))
                    {
                        EnableRaisingEvents
                            = true
                    };

                    fileSystemWatcher.Changed += (o, e) =>
                    {
                        if (Path.GetFullPath(e.FullPath) == Path.GetFullPath(srcPath))
                        {
                            autoResetEvent.Set();
                        }
                    };

                    autoResetEvent.WaitOne();
                }
            }
        }

        public static void RemoveFinishedTasks()
        {
            for (var i = 0; i < RunningTasks.Count; i++)
            {
                if (RunningTasks[i].IsCompleted || RunningTasks[i].IsCanceled || RunningTasks[i].IsFaulted)
                {
                    RunningTasks.Remove(RunningTasks[i]);
                }
            }
        }
    }
}
