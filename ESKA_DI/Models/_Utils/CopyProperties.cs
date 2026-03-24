using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace Models._Utils
{
    public class CopyProperty
    {
        public static void CopyProperties(object sourceObject, object targetObject, bool deepCopy = true, string[] exceptColumns = null)
        {
            if (exceptColumns == null)
            {

                if (sourceObject != null && targetObject != null)
                {
                    (from sourceProperty in sourceObject.GetType().GetProperties().AsEnumerable()
                     from targetProperty in targetObject.GetType().GetProperties().AsEnumerable()
                     where sourceProperty.Name.ToUpper() == targetProperty.Name.ToUpper()
                     let sourceValue = sourceProperty.GetValue(sourceObject, null)
                     //let sourceTypeName = sourceProperty.DeclaringType.FullName
                     //where sourceValue != null
                     select CopyProperty_(targetProperty, targetObject, sourceValue, deepCopy, exceptColumns))
                    .ToList()
                    .ForEach(c => c());
                }
            }
            else
            {
                if (sourceObject != null && targetObject != null)
                {
                    (from sourceProperty in sourceObject.GetType().GetProperties().AsEnumerable()
                     from targetProperty in targetObject.GetType().GetProperties().AsEnumerable()
                     where sourceProperty.Name.ToUpper() == targetProperty.Name.ToUpper()
                     let sourceValue = sourceProperty.GetValue(sourceObject, null)
                     //let sourceTypeName = sourceProperty.DeclaringType.FullName
                     where //sourceValue != null && 
                            !exceptColumns.Contains(sourceProperty.Name)
                     select CopyProperty_(targetProperty, targetObject, sourceValue, deepCopy, exceptColumns))
                    .ToList()
                    .ForEach(c => c());
                }
            }
        }

        public static Action CopyProperty_(PropertyInfo propertyInfo, object targetObject, object sourceValue, bool deepCopy, string[] exceptColumns)
        {

            //if (!deepCopy || sourceTypeName.StartsWith("System."))
            //    return () => propertyInfo.SetValue(targetObject, sourceValue, null);
            //else
            //    return () => CopyProperties(sourceValue, propertyInfo.GetValue(targetObject, null));

            if (sourceValue != null)
            {
                if (!deepCopy || sourceValue.GetType().FullName.StartsWith("System."))
                    return () => propertyInfo.SetValue(targetObject, sourceValue, null);
                else
                    return () => CopyProperties(sourceValue, propertyInfo.GetValue(targetObject, null));

            }
            else
            {
                //return () => CopyProperties(sourceValue, propertyInfo.GetValue(targetObject, null));
                return () => propertyInfo.SetValue(targetObject, sourceValue, null);
            }
        }

    }
}