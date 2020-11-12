using System.Collections.Generic;
using System.Text;

namespace KayakoRestApi.Text
{
    public class RequestBodyBuilder
    {
        private readonly StringBuilder sb;

        public RequestBodyBuilder() => this.sb = new StringBuilder();

        public RequestBodyBuilder(string value) => this.sb = new StringBuilder(value);

        public void AppendRequestData(string key, object value)
        {
            if (!string.IsNullOrEmpty(this.sb.ToString()))
            {
                this.sb.Append("&");
            }

            this.sb.AppendFormat("{0}={1}", key, value);
        }

        internal void AppendRequestDataArray<T>(string key, IEnumerable<T> values)
        {
            foreach (object value in values)
            {
                if (!string.IsNullOrEmpty(this.sb.ToString()))
                {
                    this.sb.Append("&");
                }

                this.sb.AppendFormat("{0}={1}", key, value);
            }
        }

        internal void AppendRequestDataNonEmptyString(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            this.AppendRequestData(key, value);
        }

        internal void AppendRequestDataNonNegativeInt(string key, int value)
        {
            if (value <= 0)
            {
                return;
            }

            this.AppendRequestData(key, value);
        }

        internal void AppendRequestDataBool(string key, bool? value)
        {
            if (value == null)
            {
                return;
            }

            this.AppendRequestDataBool(key, value.Value);
        }

        internal void AppendRequestDataBool(string key, bool value)
        {
            var requestValue = value ? 1 : 0;

            this.AppendRequestData(key, requestValue);
        }

        internal void AppendRequestDataArrayCommaSeparated<T>(string key, IEnumerable<T> values)
        {
            if (values == null)
            {
                return;
            }

            var sb = new StringBuilder();
            foreach (object value in values)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    sb.Append(",");
                }

                sb.Append(value);
            }

            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                this.AppendRequestData(key, sb.ToString());
            }
        }

        public override string ToString() => this.sb.ToString();
    }
}