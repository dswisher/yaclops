

namespace Yaclops.Help
{
    internal class ConsoleRenderer
    {
        private readonly IConsole _target;

        public ConsoleRenderer(IConsole target)
        {
            _target = target;
        }


        public void Render(AbstractDocumentItem source)
        {
            // TODO - capture current state of console (color, width, etc)

            // TODO - initialize renderer state?

            source.RenderTo(_target);

            // TODO - restore console, just in case we switch up colors
        }

    }
}
