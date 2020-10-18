namespace SadPumpkin.Util.LookupTree.Utils
{
    public interface ITreeDataGetPattern
    {
        string Get<T>(string path);
    }
}