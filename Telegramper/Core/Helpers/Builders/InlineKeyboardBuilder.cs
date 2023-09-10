using Telegramper.Executors;
using System.Linq.Expressions;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegramper.Core.Helpers.Builders
{
    public class InlineKeyboardBuilder
    {
        private readonly List<List<InlineKeyboardButton>> _buttons;
        private List<InlineKeyboardButton> _currentRow;

        public InlineKeyboardBuilder()
        {
            _buttons = new List<List<InlineKeyboardButton>>();
            _currentRow = new List<InlineKeyboardButton>();
        }

        public InlineKeyboardBuilder ButtonList(IEnumerable<InlineKeyboardButton> buttons, int rowCount = 1)
        {
            var queue = new Queue<InlineKeyboardButton>(buttons);

            while (queue.Count > 0)
            {
                for (int i = 0; i < rowCount && queue.Count > 0; i++)
                {
                    Button(queue.Dequeue());
                }
                EndRow();
            }

            return this;
        }

        public InlineKeyboardBuilder CallbackButtonList<T>(IEnumerable<T> list, Func<T, int, string> textConfigure,
            Func<T, int, string> callbackDataConfigure, int rowCount = 1)
        {
            var buttons = new List<InlineKeyboardButton>();
            int i = 0;
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

        public InlineKeyboardBuilder CallbackButton(string text, string callback)
        {
            _currentRow.Add(InlineKeyboardButton.WithCallbackData(text, callback));
            return this;
        }

        //public InlineKeyboardBuilder ExecutorButton(string text, Expression<Action<TExecutor>> method, string args)
        //{
        //    if (method.Body.NodeType != ExpressionType.Call)
        //        throw new ArgumentNullException("method.Body.NodeType != ExpressionType.Call");

        //    var info = (MethodCallExpression)method.Body;
        //    var methodName = info.Method.Name;
        //    CallbackButton(text, $"{methodName} {args}");

        //    return this;
        //}

        public InlineKeyboardBuilder Button(InlineKeyboardButton button)
        {
            _currentRow.Add(button);
            return this;
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
