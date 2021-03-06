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

namespace Scenarioo.Model.Docu.Entities
{
    using System.Xml.Serialization;

    using Scenarioo.Model.Docu.Entities.Generic;

    [Serializable]
    [XmlRoot("stepDescription")]
    public class StepDescription
    {
        /// <summary>
        /// Gets or sets the index needs to be the index of this step inside current scenario, starting with 0.
        /// </summary>
        [XmlElement("index")]
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the title of the current step. Usually the title shown on the page in the UI.
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the status of current step. Usually it is "success", "failed" or "unknown". Use "failed" in case an assertion
        /// failed on current step, otherwise use "success" or "unknown". you can use different application-specific strings
        /// for marking any other special states of a step.
        /// </summary>
        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("screenshotFileName")]
        public string ScreenshotFileName { get; set; }

        [XmlElement("details")]
        public Details Details { get; set; }

        public void AddDetails(string key, object value)
        {
            this.Details.AddDetail(key, value);
        }
    }
}
