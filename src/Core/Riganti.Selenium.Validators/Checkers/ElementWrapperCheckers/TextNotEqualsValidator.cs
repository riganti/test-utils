using System;
using Riganti.Selenium.Core.Abstractions;

namespace Riganti.Selenium.Validators.Checkers.ElementWrapperCheckers
{
    public class TextNotEqualsValidator : IValidator<IElementWrapper>
    {
        private readonly string text;
        private readonly bool caseSensitive;
        private readonly bool trim;

        public TextNotEqualsValidator(string text, bool caseSensitive = false, bool trim = true)
        {
            this.text = text;
            this.caseSensitive = caseSensitive;
            this.trim = trim;
        }

        public CheckResult Validate(IElementWrapper wrapper)
        {
            var wrapperText = trim ? wrapper.GetText()?.Trim() : wrapper.GetText();
            var isSucceeded = !string.Equals(wrapperText, text,
                caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
            return isSucceeded ? CheckResult.Succeeded : new CheckResult($"Element contains wrong content. Expected content: '{text}', Provided content: '{wrapperText}' \r\n Element selector: {wrapper.FullSelector} \r\n");
        }
    }
}