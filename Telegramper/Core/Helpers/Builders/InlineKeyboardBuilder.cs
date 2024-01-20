using Telegramper.Executors;
using System.Linq.Expressions;
using Telegram.Bot.Types.ReplyMarkups;
using Telegramper.Executors.Common.Options;

namespace Telegramper.Core.Helpers.Builders
{
    public class InlineKeyboardBuilder
    {
        private readonly List<List<InlineKeyboardButton>> _buttons = new();
        private List<InlineKeyboardButton> _currentRow = new();
        private readonly ParametersParserOptions _parametersParserOptions;
        
        public InlineKeyboardBuilder(ParametersParserOptions parametersParserOptions)
        {
            _parametersParserOptions = parametersParserOptions;
        }
        
        public InlineKeyboardBuilder ButtonList(IEnumerable<InlineKeyboardButton> buttons, int rowCount = 1)
        {
            var queue = new Queue<InlineKeyboardButton>(buttons);

            while (queue.Count > 0)
            {
                for (var i = 0; i < rowCount && queue.Count > 0; i++)
                {
                    Button(queue.Dequeue());
                }
                EndRow();
            }

            return this;
        }

        public InlineKeyboardBuilder ButtonRange<T>(IEnumerable<T> list, Func<T, int, string> textConfigure,
            Func<T, int, string> callbackDataConfigure, int rowCount = 1)
        {
            var buttons = new List<InlineKeyboardButton>();
            var i = 0;
            foreach (var item in list)
            {
                buttons.Add(InlineKeyboardButton.WithCallbackData(
                    textConfigure.Invoke(item, i),
                    callbackDataConfigure.Invoke(item, i)
                ));
                i++;
            }

            return ButtonList(buttons, rowCount);
        }

        public InlineKeyboardBuilder Button(string text, string callback)
        {
            _currentRow.Add(InlineKeyboardButton.WithCallbackData(text, callback));
            return this;
        }
        
        public InlineKeyboardBuilder ButtonUrl(string text, string url)
        {
            _currentRow.Add(InlineKeyboardButton.WithUrl(text, url));
            return this;
        }
        
        public InlineKeyboardBuilder Button(string textAndCallback)
        {
            _currentRow.Add(InlineKeyboardButton.WithCallbackData(textAndCallback));
            return this;
        }

        public InlineKeyboardBuilder Button(InlineKeyboardButton button)
        {
            _currentRow.Add(button);
            return this;
        }

        public InlineKeyboardBuilder Button(string text, string callbackHandler, object[] parameters)
        {
            return Button(text, 
                callbackHandler + 
                _parametersParserOptions.DefaultSeparator + 
                string.Join(_parametersParserOptions.DefaultSeparator, parameters));
        }

        public InlineKeyboardBuilder EndRow()
        {
            _buttons.Add(_currentRow);
            _currentRow = new List<InlineKeyboardButton>();
            return this;
        }

        public InlineKeyboardMarkup Build()
        {
            if (_currentRow.Count != 0)
                EndRow();

            return new InlineKeyboardMarkup(_buttons);
        }
    }
}
