using System;
using System.Collections.Generic;
using System.Text;

namespace SiteBuilder.SiteData
{
    public enum KindOfRoadMapItem
    {
        Unknown,
        Group,
        /// <summary>
        /// It is like Sprint in SCRUM
        /// </summary>
        Step,
        /// <summary>
        /// These are features without expected date of implementation.
        /// </summary>
        Unplanned,
    }
}
