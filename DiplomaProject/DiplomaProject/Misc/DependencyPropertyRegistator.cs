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
            return DependencyProperty.Register(name, typeof (TT), typeof (T), new PropertyMetadata(defaultValue, propertyChanged));
        }
        public DependencyPropertyKey RegisterReadOnly<TT>(string name, TT defaultValue = default(TT), PropertyChangedCallback propertyChanged = null) {
            return DependencyProperty.RegisterReadOnly(name, typeof(TT), typeof(T), new PropertyMetadata(defaultValue, propertyChanged));
        }  
    }
}