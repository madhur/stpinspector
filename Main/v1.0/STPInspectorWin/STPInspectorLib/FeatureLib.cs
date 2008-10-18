using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Administration;
namespace STPInspectorLib
{
    /// <summary>
    /// Processes and matches the features installed on the machine
    /// </summary>
    public class FeatureLib
    {
        /// <summary>
        /// Resolves the features installed on local machine
        /// </summary>
        /// <param name="featureItems">List of features</param>
        /// <returns>TRUE if one of the feature was unresolved</returns>
        public static bool ResolveFeatures(List<FeatureItem> featureItems)
        {
            SPFarm farmObj=SPFarm.Local;
            bool unresolvedStatus = false;
            foreach(FeatureItem featureItem in featureItems)
            {
                if (farmObj.FeatureDefinitions[new Guid(featureItem.ID)] != null)
                {
                    featureItem.Name = farmObj.FeatureDefinitions[new Guid(featureItem.ID)].DisplayName;
                }
                else
                {
                    unresolvedStatus = true;
                }
            }

            return unresolvedStatus;
          
        }

        /// <summary>
        /// Private Constructor
        /// </summary>
        private FeatureLib()
        {
            
        }
    }
}
