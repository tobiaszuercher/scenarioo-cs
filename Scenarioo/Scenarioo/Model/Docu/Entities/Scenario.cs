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

namespace Scenarioo.Model.Docu.Entities
{
    using System;
    using System.Xml.Serialization;

    using Scenarioo.Api.Util.Xml;
    using Scenarioo.Model.Docu.Entities.Generic;

    [Serializable]
    [XmlRoot("scenario")]
    public class Scenario
    {
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns;

        public Scenario()
        {
            // Set appropriate namespace
            this.Xmlns = new XmlSerializerNamespaces();
            this.Xmlns.Add("ns3", ScenarioDocuXMLFileUtil.ScenarioNameSpace);
            this.Xmlns.Add("xs", ScenarioDocuXMLFileUtil.XmlSchema);

            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Status = string.Empty;
        }

        public Scenario(string name, string description, int numberOfPages, int numberOfSteps)
        {
            this.Name = name;
            this.Description = description;
        }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("details")]
        public Details Details { get; set; }

        public void AddDetail(string key, object value)
        {
            if (this.Details == null)
            {
                this.Details = new Details();
            }

            this.Details.AddDetail(key, value);
        }

    }
}
