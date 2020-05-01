using System;
using LookupTree.Payloads;
using Newtonsoft.Json;

namespace LookupTree.Utils
{
    public class TreeSaver
    {
        private readonly ITreePathGetPattern _pathGetPattern = null;
        private readonly ITreeDataSetPattern _dataSetPattern = null;

        public TreeSaver(ITreePathGetPattern pathGetPattern, ITreeDataSetPattern dataSetPattern)
        {
            _pathGetPattern = pathGetPattern;
            _dataSetPattern = dataSetPattern;
        }

        public bool SaveTree<T>(LookupTree<T> tree) where T : class, IPayload
        {
            if (tree == null)
                return false;

            try
            {
                string treePath = _pathGetPattern.Get<T>();
                string treeData = JsonConvert.SerializeObject(
                    tree,
                    new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                        Formatting = Formatting.Indented
                    });
                if (string.IsNullOrWhiteSpace(treeData))
                    throw new NullReferenceException("Tree data is null.");

                return _dataSetPattern.Set<T>(treePath, treeData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save tree failed. [{ex.GetType().Name}] {ex.Message}");
                return false;
            }
        }
    }
}