using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.EditorScene.Command
{
    public interface ICommandManager
    {
        void ExecuteCommand(ICommand command);
        void UndoLastCommand();
    }

    public class CommandManager : ICommandManager
    {
        private readonly Stack<ICommand> _executedCommands = new();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute(g =>
            {
                Debug.Log($"Execute on {g.gameObject}");
                _executedCommands.Push(command);
                Log();
            });
        }

        private void Log()
        {
            Debug.Log($"------Commands--{_executedCommands.Count}----------");
            
            foreach (var com in _executedCommands)
            {
                Debug.Log(com.ToString());
            }
        }

        public void UndoLastCommand()
        {
            if (_executedCommands.Count > 0)
            {
                var lastCommand = _executedCommands.Pop();
                lastCommand.Undo(g =>
                {
                    Log();
                });
            }
        }
    }
}