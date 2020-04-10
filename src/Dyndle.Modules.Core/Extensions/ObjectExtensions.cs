using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace Dyndle.Modules.Core.Extensions
{
    /// <summary>
    /// Extension methods.
    /// Extending <seealso cref="object"/>
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Throws if null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ThrowIfNull<T>(this T obj, string parameterName)
               where T : class
        {
            if (obj == null) throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Throws an NullArgument exception if obj is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ThrowIfNull<T>(this T obj, string message, string parameterName)
               where T : class
        {
            if (obj == null) throw new ArgumentNullException(message, parameterName);
        }

        /// <summary>
        /// Determines whether the specified object is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if the specified object is null; otherwise, <c>false</c>.</returns>
        public static bool IsNull<T>(this T obj)
              where T : class
        {
            return (obj == null);
        }

        /// <summary>
        /// Uses recursion and reflection to print the bindable layout of this object
        /// </summary>
        /// <param name="myObject">My object.</param>
        /// <param name="displaySubObject">Include subobjects?</param>
        /// <param name="includeTypeName">Include typenames?</param>
        /// <returns>System.String.</returns>
        public static string Layout(this object myObject, bool displaySubObject, bool includeTypeName)
        {
            return myObject.Layout(displaySubObject, includeTypeName, string.Empty, new StringWriter());
        }

        private static string Layout(this object myObject, bool displaySubObject, bool includeTypeName, string prefix, StringWriter writer)
        {
            if (myObject != null)
            {
                Type objectType = myObject.GetType();
                //check for collection
                if (objectType.GetInterface("IEnumerable") != null)
                {
                    int index = 0;
                    IEnumerable list = (IEnumerable)myObject;
                    foreach (object item in list)
                    {
                        item.Layout(displaySubObject, includeTypeName, string.Format("{0}[{1}]", prefix, index), writer);
                        index += 1;
                    }
                }
                else
                {
                    ArrayList al = new ArrayList();
                    PropertyInfo pi = default(PropertyInfo);
                    FieldInfo fi = default(FieldInfo);
                    MemberInfo[] members = objectType.GetMembers();

                    foreach (MemberInfo mi in members)
                    {
                        if ((mi.MemberType & MemberTypes.Constructor) != 0) { /*ignore constructor*/}
                        else if (object.ReferenceEquals(mi.DeclaringType, typeof(object))) { /*ignore inherited*/}
                        else if (!al.Contains(mi.Name) && (mi.MemberType & MemberTypes.Property) != 0)
                        {
                            al.Add(mi.Name);
                            pi = (System.Reflection.PropertyInfo)mi;

                            if (!(displaySubObject) || (pi.PropertyType.IsValueType || pi.PropertyType.Equals(typeof(string))))
                            {
                                if (includeTypeName)
                                {
                                    writer.WriteLine(string.Format("{0}.{1} ({2})", prefix, pi.Name, pi.PropertyType.Name).TrimStart('.'));
                                }
                                else
                                {
                                    writer.WriteLine(string.Format("{0}.{1}", prefix, pi.Name).TrimStart('.'));
                                }
                            }
                            else
                            {
                                //display sub objects
                                pi.GetValue(myObject, null).Layout(displaySubObject, includeTypeName, string.Format("{0}.{1}", prefix, pi.Name), writer);
                            }
                        }
                        else if (!al.Contains(mi.Name) && (mi.MemberType & MemberTypes.Field) != 0)
                        {
                            al.Add(mi.Name);
                            fi = (System.Reflection.FieldInfo)mi;

                            if (!(displaySubObject) || (fi.FieldType.IsValueType || fi.FieldType.Equals(typeof(string))))
                            {
                                if (includeTypeName)
                                {
                                    writer.WriteLine(string.Format("{0}.{1} ({2})", prefix, fi.Name, fi.FieldType.Name).TrimStart('.'));
                                }
                                else
                                {
                                    writer.WriteLine(string.Format("{0}.{1}", prefix, fi.Name).TrimStart('.'));
                                }
                            }
                            else
                            {
                                //display sub objects
                                fi.GetValue(myObject).Layout(displaySubObject, includeTypeName, string.Format("{0}.{1}", prefix, fi.Name), writer);
                            }
                        }
                    }
                }
            }
            return writer.ToString();
        }
    }
}