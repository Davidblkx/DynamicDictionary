namespace Dynamic
{
    public class DynamicListValueChangedArgs
    {
        public DynamicDictionaryChangedType EventType { get; set; }
        public object OldValue { get; set; }
        public object Value { get; set; }
    }
}
