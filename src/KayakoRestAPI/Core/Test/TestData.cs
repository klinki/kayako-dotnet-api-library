using System;

namespace KayakoRestApi.Core.Test
{
    [Serializable]
    public class TestData
    {
        public TestData() { }

        public TestData(string data) => this.DataValue = data;

        private string DataValue { get; set; }

        public string Data => this.DataValue;

        public static implicit operator string(TestData testData) => testData.Data;

        public override string ToString() => this.DataValue;
    }
}