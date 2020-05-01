using System;
using System.Collections.Generic;
using LookupTree.Payloads;
using Newtonsoft.Json;

namespace LookupTree.Utils
{
    public class TreeLoader
    {
        private readonly ITreePathGetPattern _pathGetPattern = null;
        private readonly ITreeDataGetPattern _dataGetPattern = null;
        private readonly Dictionary<Type, object> _cachedTrees = new Dictionary<Type, object>();

        public TreeLoader(ITreePathGetPattern pathGetPattern, ITreeDataGetPattern dataGetPattern)
        {
            _pathGetPattern = pathGetPattern;
            _dataGetPattern = dataGetPattern;
        }

        public void ClearCache()
        {
            _cachedTrees.Clear();
        }

        public bool TryLoadTree<T>(out LookupTree<T> loadedTree) where T : class, IPayload
        {
            loadedTree = null;

            Type type = typeof(T);

            if (_cachedTrees.TryGetValue(type, out object obj) &&
                obj is LookupTree<T> tree)
            {
                loadedTree = tree;
                return true;
            }

            try
            {
                string treePath = _pathGetPattern.Get<T>();
                string treeData = _dataGetPattern.Get<T>(treePath);
                if (string.IsNullOrWhiteSpace(treeData))
                    throw new NullReferenceException("Tree data is null.");

                loadedTree = JsonConvert.DeserializeObject<LookupTree<T>>(
                    treeData,
                    new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                        Formatting = Formatting.Indented
                    });

                _cachedTrees[type] = loadedTree ?? throw new NullReferenceException("Loaded tree is null.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Load tree failed. [{ex.GetType().Name}] {ex.Message}");
                return false;
            }
        }
    }
}