namespace Field
{
    public class FieldDataFromFile
    {
        private Config _config;
        
        public FieldDataFromFile(Config config)
        {
            _config = config;
        }

        public string GetData()
        {
            var textAsset = _config.FieldTextAsset;
            return textAsset.text;
        }
    }
}