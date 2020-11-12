using System;
using System.Linq;
using KayakoRestApi.RequestBase.Attributes;

namespace KayakoRestApi.RequestBase
{
    public abstract class RequestBaseObject
    {
        public bool IsValid(RequestTypes requestType)
        {
            try
            {
                this.EnsureValidData(requestType);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Validates the request base object for a request type (create or update)
        /// </summary>
        public void EnsureValidData(RequestTypes requestType)
        {
            var properties = this.GetType().GetProperties();

            foreach (var info in properties)
            {
                if (info.GetCustomAttributes(typeof(RequiredFieldAttribute), false) is RequiredFieldAttribute[] required && required.Length > 0)
                {
                    var checkValue = required.Length == 0 || required[0].RequestTypes.Any(type => type == requestType);

                    if (checkValue && info.GetValue(this, null) == null)
                    {
                        throw new ArgumentException(string.Format("{0} - Required field missing", this.GetType().Name), info.Name);
                    }
                }

                if (info.GetCustomAttributes(typeof(EitherFieldAttribute), false) is EitherFieldAttribute[] eitherField && eitherField.Length > 0)
                {
                    if (info.GetValue(this, null) != null)
                    {
                        foreach (var prop in eitherField[0].DependsOn)
                        {
                            var pInfo = this.GetType().GetProperty(prop);

                            if (pInfo != null)
                            {
                                if (pInfo.GetValue(this, null) != null)
                                {
                                    throw new ArgumentException(string.Format("If {0} has a value, {1} must be null", info.Name, pInfo.Name), info.Name);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Converts from an Api response object to a request base object
        /// </summary>
        protected static TTo FromResponseType<TFrom, TTo>(TFrom responseObject)
            where TTo : RequestBaseObject
        {
            var ci = typeof(TTo).GetConstructor(new Type[0]);

            var output = (TTo) ci.Invoke(new object[0]);

            foreach (var info in typeof(TTo).GetProperties())
            {
                if (info.GetCustomAttributes(typeof(ResponsePropertyAttribute), false) is ResponsePropertyAttribute[] responseMatchAtt && responseMatchAtt.Length > 0)
                {
                    var matchingProp = responseObject.GetType().GetProperty(responseMatchAtt[0].RepsonseProperty);

                    info.SetValue(output, matchingProp.GetValue(responseObject, null), null);
                }
            }

            return output;
        }

        /// <summary>
        ///     Converts from an request base object to an Api response object
        /// </summary>
        protected static TTo ToResponseType<TFrom, TTo>(TFrom requestObject)
            where TFrom : RequestBaseObject
        {
            var ci = typeof(TTo).GetConstructor(new Type[0]);

            var output = (TTo) ci.Invoke(new object[0]);

            foreach (var info in typeof(TFrom).GetProperties())
            {
                if (info.GetCustomAttributes(typeof(ResponsePropertyAttribute), false) is ResponsePropertyAttribute[] responseMatchAtt && responseMatchAtt.Length > 0)
                {
                    var matchingProp = output.GetType().GetProperty(responseMatchAtt[0].RepsonseProperty);

                    matchingProp.SetValue(output, info.GetValue(requestObject, null), null);
                }
            }

            return output;
        }
    }
}