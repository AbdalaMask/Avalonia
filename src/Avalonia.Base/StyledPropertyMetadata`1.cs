using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Data;

#nullable enable

namespace Avalonia
{
    /// <summary>
    /// Metadata for styled avalonia properties.
    /// </summary>
    public class StyledPropertyMetadata<TValue> : PropertyMetadata, IStyledPropertyMetadata
    {
        private Optional<TValue> _defaultValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="StyledPropertyMetadata{TValue}"/> class.
        /// </summary>
        /// <param name="defaultValue">The default value of the property.</param>
        /// <param name="defaultBindingMode">The default binding mode.</param>
        /// <param name="coerce">A value coercion callback.</param>
        public StyledPropertyMetadata(
            Optional<TValue> defaultValue = default,
            BindingMode defaultBindingMode = BindingMode.Default,
            Func<IAvaloniaObject, TValue, TValue>? coerce = null)
                : base(defaultBindingMode)
        {
            _defaultValue = defaultValue;
            CoerceValue = coerce;
        }

        /// <summary>
        /// Gets the default value for the property.
        /// </summary>
        [MaybeNull]
        public TValue DefaultValue => _defaultValue.GetValueOrDefault();

        /// <summary>
        /// Gets the value coercion callback, if any.
        /// </summary>
        public Func<IAvaloniaObject, TValue, TValue>? CoerceValue { get; private set; }

        object? IStyledPropertyMetadata.DefaultValue => DefaultValue;

        /// <inheritdoc/>
        public override void Merge(PropertyMetadata baseMetadata, AvaloniaProperty property)
        {
            base.Merge(baseMetadata, property);

            if (baseMetadata is StyledPropertyMetadata<TValue> src)
            {
                if (!_defaultValue.HasValue)
                {
                    _defaultValue = new Optional<TValue>(src.DefaultValue);
                }

                if (CoerceValue == null)
                {
                    CoerceValue = src.CoerceValue;
                }
            }
        }
    }
}
