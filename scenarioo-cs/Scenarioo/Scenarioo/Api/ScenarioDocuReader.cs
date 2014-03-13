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

namespace Scenarioo.Api
{
    using Scenarioo.Api.Files;
    using Scenarioo.Api.Util.Xml;
    using Scenarioo.Model.Docu.Entities;

    public class ScenarioDocuReader
    {
        private readonly ScenarioDocuFiles docuFiles;

        public ScenarioDocuReader(string rootDirectory)
        {
            this.docuFiles = new ScenarioDocuFiles(rootDirectory);
        }

        public branch LoadBranch(string buildName, string branchName)
        {
            return ScenarioDocuXMLFileUtil.Unmarshal<branch>(
                docuFiles.GetBranchFile(buildName, branchName));
        }

        public build LoadBuild(string buildName, string branchName)
        {
            return ScenarioDocuXMLFileUtil.Unmarshal<build>(
                docuFiles.GetBuildFile(buildName));
        }

        public useCase LoadUseCase(string buildName, string branchName, string useCaseName)
        {
            return ScenarioDocuXMLFileUtil.Unmarshal<useCase>(
                docuFiles.GetUseCaseFile(buildName, branchName, useCaseName));
        }

        public scenario LoadScenario(string buildName, string branchName, string useCaseName, string scenarioName)
        {
            return ScenarioDocuXMLFileUtil.Unmarshal<scenario>(
                docuFiles.GetScenarioFile(buildName, branchName, useCaseName, scenarioName));
        }
    }

}
