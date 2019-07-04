using System.Collections;
using System.Linq;

namespace Calc
{

    class Calculator
    {
        // Допустимые клавиши
        public enum KeyType : byte { dot, zero, one, two, three, four, five, six, seven, eight, nine, clear, delete, equal, plus, minus, multiply, divide };
        // Допустимые команды
        enum CmdType : byte { Undef, Plus, Minus, Multiply, Divide };
        // Действия
        ArrayList actions;
        // Операнд
        string operand;
        // Команда
        CmdType command;

        // Конструктор
        public Calculator()
        {
            actions = new ArrayList(20);
            Clear();
        }

        // Текущее значение
        public override string ToString()
        {
            string DecodeCommand(object cmd)
            {
                switch (cmd)
                {
                    case CmdType.Plus:
                        return "+";
                    case CmdType.Minus:
                        return "-";
                    case CmdType.Multiply:
                        return "*";
                    case CmdType.Divide:
                        return "/";
                    default:
                        return "";
                }
            }

            string result = "";

            foreach (var item in actions)
                result += (item.GetType() == typeof(string) ? item : DecodeCommand(item));

            if (operand != "")
                result += operand;

            if (command != CmdType.Undef)
                result += DecodeCommand(command);

            return result;
        }

        // Очистить
        void Clear()
        {
            actions.Clear();
            operand = "";
            command = CmdType.Undef;
        }

        // Удалить
        void Delete()
        {
            if (operand == "" & command == CmdType.Undef)
                return;

            if (operand != "")
            {
                operand = operand.Remove(operand.Length - 1);

                if (operand == "" & command == CmdType.Undef & actions.Count > 0)
                {
                    command = (CmdType)actions[actions.Count - 1];
                    actions.RemoveAt(actions.Count - 1);
                    return;
                }

                return;
            }

            if (command != CmdType.Undef)
            {
                command = CmdType.Undef;

                if (operand == "" & actions.Count > 0)
                {
                    operand = (string)actions[actions.Count - 1];
                    actions.RemoveAt(actions.Count - 1);
                    return;
                }

                return;
            }
        }


        void AddOperand()
        {
            if (operand != "")
            {
                actions.Add(operand);
                operand = "";
            }
        }

        void AddCommand()
        {
            if (command != CmdType.Undef)
            {
                actions.Add(command);
                command = CmdType.Undef;
            }
        }


        // Равно
        void Equal()
        {
            // Добавляем последний операнд или команду в массив
            AddOperand();
            AddCommand();

            // Проверка на наличие чего либо
            if (actions.Count == 0)
                return;

            // Результат вычислений
            double result = 0;
            double leftOperand;
            double rightOperand;

            // Умножение/деление
            while (actions.Contains(CmdType.Multiply) || actions.Contains(CmdType.Divide))
            {
                for (int i = 0; i < actions.Count; i++)
                    if (actions[i].GetType() == typeof(CmdType) && ((CmdType)actions[i] == CmdType.Divide || (CmdType)actions[i] == CmdType.Multiply))
                    {
                        // Первый операнд
                        leftOperand = double.Parse((string)actions[i - 1]);
                        // Второй операнд
                        if (i < actions.Count - 1)
                            rightOperand = double.Parse((string)actions[i + 1]);
                        else
                            rightOperand = 1.0;
                        // Результат вычисления
                        switch ((CmdType)actions[i])
                        {
                            case CmdType.Multiply:
                                result = leftOperand * rightOperand;
                                break;
                            case CmdType.Divide:
                                if (rightOperand == 0)
                                {
                                    Clear();
                                    operand = "Error";
                                    return;
                                }
                                result = leftOperand / rightOperand;
                                break;
                        }
                        // Удаляем правый операнд
                        if (i < actions.Count - 1)
                            actions.RemoveAt(i + 1);
                        // Удаляем оператор    
                        actions.RemoveAt(i);
                        // Замещаем левый операнд
                        actions[i - 1] = result.ToString();

                        break;
                    }
            }

            // сложение/вычитание
            while (actions.Contains(CmdType.Plus) || actions.Contains(CmdType.Minus))
            {
                for (int i = 0; i < actions.Count; i++)
                    if (actions[i].GetType() == typeof(CmdType) && ((CmdType)actions[i] == CmdType.Plus || (CmdType)actions[i] == CmdType.Minus))
                    {
                        // Первый операнд
                        if (i == 0)
                            leftOperand = ((CmdType)actions[i] == CmdType.Minus) ? 0.0 : 1.0;
                        else
                            leftOperand = double.Parse((string)actions[i - 1]);

                        // Второй операнд
                        if (i < actions.Count - 1)
                            rightOperand = double.Parse((string)actions[i + 1]);
                        else
                            rightOperand = 0.0;
                        // Результат вычисления
                        switch ((CmdType)actions[i])
                        {
                            case CmdType.Plus:
                                result = leftOperand + rightOperand;
                                break;
                            case CmdType.Minus:
                                result = leftOperand - rightOperand;
                                break;
                        }
                        // Удаляем правый операнд
                        if (i < actions.Count - 1)
                            actions.RemoveAt(i + 1);
                        // Удаляем оператор    
                        actions.RemoveAt(i);
                        // Замещаем левый операнд
                        if (i - 1 < 0)
                            actions.Add(result.ToString());
                        else
                            actions[i - 1] = result.ToString();

                        break;
                    }
            }

            // Результат переносим в операнд
            operand = (string)actions[0];
            // Чистим actions
            actions.Clear();
        }


        // Новый символ
        void NewChar(char c)
        {
            // Добавляем команду в массив
            AddCommand();

            // Проверка допустимости символа
            if (c == ',' & operand.Contains(',') || c == '0' & operand == "0")
                return;

            operand += c;
        }


        void NewCmd(CmdType cmd)
        {
            // Добавляем операнд в массив
            AddOperand();

            if (actions.Count != 0 | cmd == CmdType.Minus | cmd == CmdType.Plus)
                command = cmd;
        }

        // Добавить кнопку
        public void AddKey(KeyType key)
        {
            // Убираем ошибку
            if (operand == "Error")
                operand = "";

            switch (key)
            {
                case KeyType.clear:
                    Clear();
                    break;
                case KeyType.delete:
                    Delete();
                    break;
                case KeyType.equal:
                    Equal();
                    break;
                case KeyType.dot:
                    NewChar(',');
                    break;
                case KeyType.zero:
                    NewChar('0');
                    break;
                case KeyType.one:
                    NewChar('1');
                    break;
                case KeyType.two:
                    NewChar('2');
                    break;
                case KeyType.three:
                    NewChar('3');
                    break;
                case KeyType.four:
                    NewChar('4');
                    break;
                case KeyType.five:
                    NewChar('5');
                    break;
                case KeyType.six:
                    NewChar('6');
                    break;
                case KeyType.seven:
                    NewChar('7');
                    break;
                case KeyType.eight:
                    NewChar('8');
                    break;
                case KeyType.nine:
                    NewChar('9');
                    break;
                case KeyType.plus:
                    NewCmd(CmdType.Plus);
                    break;
                case KeyType.minus:
                    NewCmd(CmdType.Minus);
                    break;
                case KeyType.multiply:
                    NewCmd(CmdType.Multiply);
                    break;
                case KeyType.divide:
                    NewCmd(CmdType.Divide);
                    break;
            }
        }
    }
}
