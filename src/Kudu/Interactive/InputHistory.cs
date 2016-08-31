using System.Collections.Generic;

namespace Kudu.Interactive
{
    public class InputHistory
    {
        private int _index;
        private readonly int _limit = 50;
        private readonly List<string> _history = new List<string>();

        public InputHistory(int limit)
        {
            _limit = limit;
            Reset();
        }

        public void Add(string input)
        {
            _history.Add(input);
            if (_history.Count > _limit)
            {
                _history.RemoveAt(0);
            }
        }

        public void Reset()
        {
            _index = _history.Count;
        }

        public string Next()
        {
            if (_history.Count == 0 || _index >= _history.Count)
            {
                return null;
            }
            _index++;
            return _index >= _history.Count ? string.Empty : _history[_index];
        }

        public string Previous()
        {
            if (_history.Count == 0 || _index < 0)
            {
                return null;
            }
            _index--;
            return _index < 0 ? string.Empty : _history[_index];
        }
    }
}