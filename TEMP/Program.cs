using System.Text;

State sta = new();
sta.home = new() { "祥子", "立希", "灯", "爱音", "素世" };
sta.Sort();
sta.loc = 1;
SortedSet<State> visited = new(new Cmp());
Rec(sta);
Console.WriteLine("终.");

void Rec(State x)
{
    if (!visited.Add(x))
        return;
    switch (x.loc)
    {
        case 1:
            while (x.boat.Count != 0)
            {
                x.home.Add(x.boat[0]);
                x.boat.RemoveAt(0);
            }
            if (!x.ACC()) return;
            for (int i = 0; i < x.home.Count; i++)
            {
                for (int j = i + 1; j < x.home.Count; j++)
                {
                    var m1 = new State(x);
                    m1.home.RemoveAt(j);
                    m1.home.RemoveAt(i);
                    m1.boat.Add(x.home[i]);
                    m1.boat.Add(x.home[j]);
                    if (!m1.ACC()) continue;
                    m1.loc = 2;
                    m1.Sort();
                    m1.GoLog();
                    Rec(m1);
                }
                var m2 = new State(x);
                m2.home.RemoveAt(i);
                m2.boat.Add(x.home[i]);
                if (!m2.ACC()) continue;
                m2.loc = 2;
                m2.Sort();
                m2.GoLog();
                Rec(m2);
            }
            break;
        case 2:
            while (x.boat.Count != 0)
            {
                x.des.Add(x.boat[0]);
                x.boat.RemoveAt(0);
            }
            if (!x.ACC()) return;
            if (x.des.Count == 5)
            {
                x.GoLog();
                Console.Write(x.log.ToString());
                Console.WriteLine("成功到达。");
                Console.WriteLine();
                return;
            }
            for (int i = 0; i < x.des.Count; i++)
            {
                for (int j = i + 1; j < x.des.Count; j++)
                {
                    var m1 = new State(x);
                    m1.des.RemoveAt(j);
                    m1.des.RemoveAt(i);
                    m1.boat.Add(x.des[i]);
                    m1.boat.Add(x.des[j]);
                    if (!m1.ACC()) continue;
                    m1.loc = 1;
                    m1.Sort();
                    m1.BackLog();
                    Rec(m1);
                }
                var m2 = new State(x);
                m2.des.RemoveAt(i);
                m2.boat.Add(x.des[i]);
                if (!m2.ACC()) continue;
                m2.loc = 1;
                m2.Sort();
                m2.BackLog();
                Rec(m2);
            }  
            break;
    }
}

class State : IComparable<State>
{
    public List<string> home, boat, des;
    public int loc;
    public StringBuilder log;
    public State()
    {
        home = new();
        boat = new();
        des = new();
        log = new();
        loc = 1;
    }

    public State(State state)
    {
        home = [.. state.home];
        boat = [.. state.boat];
        des = [.. state.des];
        log = new(state.log.ToString());
        loc = state.loc;
    }


    public int Nothing { get; set; }


    bool One_ACC(List<string> q, int type)
    {
        if (q.Count == 2 && q.Contains("立希") && !q.Contains("祥子")) return false;
        if (q.Count == 2 && q.Contains("灯") && q.Contains("爱音")) return false;
        if (q.Count == 2 && q.Contains("灯") && q.Contains("祥子")) return false;
        if (q.Count == 2 && q.Contains("素世") && q.Contains("祥子")) return false;
        if (type == 2 && q.Count == 1 && q[0] == "祥子") return false;
        return true;
    }

    public bool ACC()
    {
        return One_ACC(home, 1) && One_ACC(boat, 2) && One_ACC(des, 3);
    }
    public void Sort()
    {
        home.Sort();
        boat.Sort();
        des.Sort();
        CmpName = ToString();
    }

    public string CmpName { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append("起点:");
        foreach (var i in home)
        {
            sb.Append(i);
            sb.Append(' ');
        }
        sb.Append("船上:");
        foreach (var i in boat)
        {
            sb.Append(i);
            sb.Append(' ');
        }

        sb.Append("终点:");
        foreach (var i in des)
        {
            sb.Append(i);
            sb.Append(' ');
        }

        sb.Append($"loc:{loc}");

        return sb.ToString();
    }

    public int CompareTo(State? st)
    {
        var t = this;
        if (t.loc != st.loc) return t.loc - st.loc;
        if (t.home.Count != st.home.Count) return t.home.Count - st.home.Count;
        if (t.boat.Count != st.boat.Count) return t.boat.Count - st.boat.Count;
        for (int i = 0; i < home.Count; i++)
            if (t.home[i] != st.home[i]) return t.home[i].CompareTo(st.home[i]);
        for (int i = 0; i < boat.Count; i++)
            if (t.boat[i] != st.boat[i]) return t.boat[i].CompareTo(st.boat[i]);
        return 0;
    }

    public int logTimes = 0;

    public void GoLog()
    {
        StringBuilder sb = new();
        foreach (var c in home)
            sb.Append(c+' ');
        sb.Append("→ ");
        foreach (var c in boat)
            sb.Append(c + ' ');
        sb.Append("→ ");
        foreach (var c in des)
            sb.Append(c + ' ');
        log.AppendLine(sb.ToString());
        logTimes++;
    }

    public void BackLog()
    {
        StringBuilder sb = new();
        foreach (var c in home)
            sb.Append(c + ' ');
        sb.Append("← ");
        foreach (var c in boat)
            sb.Append(c + ' ');
        sb.Append("← ");
        foreach (var c in des)
            sb.Append(c + ' ');
        log.AppendLine(sb.ToString());
        logTimes++;
    }
}

class Cmp : IComparer<State>
{
    public int Compare(State? t, State? st)
    {
        return t.CmpName.CompareTo(st.CmpName);
    }
}