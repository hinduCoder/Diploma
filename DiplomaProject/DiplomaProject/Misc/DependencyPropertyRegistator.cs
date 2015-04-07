using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;

namespace DiplomaProject
{
    public class DependencyPropertyRegistator<T>
    {
        public DependencyProperty Register<TT>(string name, TT defaultValue = default(TT), PropertyChangedCallback propertyChanged = null)
        {
//            var member = ((MemberExpression)nameExpression.Body).Member;
//            var name = ((PropertyInfo)member).Name;
            return DependencyProperty.Register(name, typeof (TT), typeof (T), new PropertyMetadata(defaultValue, propertyChanged));
        }  
    }
}