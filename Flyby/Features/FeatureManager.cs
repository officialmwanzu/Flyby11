using Features;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flyoobe
{
    /// <summary>
    /// Manages all available features and categories.
    /// Provides methods to query, apply, and undo tweaks.
    /// </summary>
    public class FeatureManager
    {
        // List of all loaded features (root nodes with category and children)
        public List<FeatureNode> AllFeatures { get; private set; }

        public FeatureManager()
        {
            // Load features from loader once during construction
            AllFeatures = FeatureLoader.Load();
        }

        /// <summary>
        /// Returns all available top-level categories.
        /// </summary>
        public List<string> GetCategories()
        {
            return AllFeatures.Select(f => f.Category).ToList();
        }

        /// <summary>
        /// Returns all tweaks under a specific category.
        /// </summary>
        public List<FeatureNode> GetFeaturesByCategory(string category)
        {
            return AllFeatures.FirstOrDefault(f => f.Category == category)?.Children ?? new List<FeatureNode>();
        }

        /// <summary>
        /// Applies (executes) a selected tweak asynchronously.
        /// </summary>
        public async Task<bool> ApplyFeature(FeatureNode node)
        {
            return await node.Feature.DoFeature();
        }

        /// <summary>
        /// Undoes a tweak if supported.
        /// </summary>
        public bool UndoFeature(FeatureNode node)
        {
            return node.Feature.UndoFeature();
        }

        /// <summary>
        /// Returns the short informational description of the feature.
        /// </summary>
        public string GetFeatureInfo(FeatureNode node)
        {
            return node.Feature?.Info() ?? "";
        }

        /// <summary>
        /// Checks if the tweak is currently enabled (active).
        /// </summary>
        public async Task<bool> IsEnabled(FeatureNode node)
        {
            return await node.Feature.CheckFeature();
        }

        /// <summary>
        /// Collects all features from all categories that are marked as "Recommended" 
        /// Use quick settings option.
        /// </summary>
        public List<FeatureNode> GetAllRecommendedFeatures()
        {
            var recommended = new List<FeatureNode>();
            foreach (var categoryNode in AllFeatures)
            {
                foreach (var featureNode in categoryNode.Children)
                {
                    if (featureNode.Feature.IsRecommended)
                    {
                        recommended.Add(featureNode);
                    }
                }
            }
            return recommended;
        }



    }
}
