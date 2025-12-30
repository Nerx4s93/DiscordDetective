using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using PipeScript.API;
using PipeScript.Highlighter;

namespace PipeScript.GUI;

public sealed partial class ScriptForm : Form, IScriptHost
{
    private PipeScriptEngine _pipeScriptEngine = null!;
    private ScriptDebugger _debugger = null!;
    private SyntaxHighlighter _syntaxHighlighter = null!;
    private string _scriptName = null!;

    private bool _isPaused;

    public ScriptForm() : this("unnamed", "") { }

    public ScriptForm(string code) : this("unnamed", code) { }

    public ScriptForm(string scriptName, string code)
    {
        InitializeComponent();
        AdjustScriptEngine(scriptName, code);
        AdjustHighlighter(richTextBoxCode);
    }

    #region Запуск

    private void AdjustScriptEngine(string scriptName, string code)
    {
        richTextBoxCode.Text = code;
        Text = scriptName;

        _scriptName = scriptName;

        _debugger = new ScriptDebugger();

        _pipeScriptEngine = new PipeScriptEngine(scriptName, _debugger)
        {
            Context =
            {
                Host = new WinFormsHost(this)
            }
        };

        _debugger.AttachCallStack(_pipeScriptEngine.CallStack);

        _pipeScriptEngine.Started += OnStarted;
        _pipeScriptEngine.Finished += OnFinished;
        _pipeScriptEngine.Stopped += OnStopped;
        _pipeScriptEngine.Error += OnError;
    }

    private void AdjustHighlighter(RichTextBox richTextBox)
    {
        var commands = new CommandRegistry().CommandNames;

        _syntaxHighlighter = new SyntaxHighlighter(richTextBox);

        // команды
        _syntaxHighlighter.AddPattern(new PatternDefinition(commands), new SyntaxStyle(Color.DeepPink));

        // комментарии
        _syntaxHighlighter.AddPattern(new PatternDefinition(new Regex(@";.*?$", RegexOptions.Multiline | RegexOptions.Compiled)), new SyntaxStyle(Color.DarkGray, false, true));

        // переменные ($name)
        _syntaxHighlighter.AddPattern(new PatternDefinition(@"\$[a-zA-Z_]\w*"), new SyntaxStyle(Color.FromArgb(31, 55, 127)));

        // числа
        _syntaxHighlighter.AddPattern(new PatternDefinition(@"\d+\.\d+|\d+"), new SyntaxStyle(Color.Purple));

        // операторы
        _syntaxHighlighter.AddPattern(new PatternDefinition("(", ")", "*", "/", "+", "-", ">", "<", "&", "|"), new SyntaxStyle(Color.Brown));
    }

    #endregion

    #region Событие движка

    private void UpdateButtons()
    {
        buttonStop.Enabled = _pipeScriptEngine.IsRunning;
        buttonStep.Enabled = _pipeScriptEngine.IsRunning && _isPaused;
        buttonStepOver.Enabled = _pipeScriptEngine.IsRunning && _isPaused;
        richTextBoxCode.ReadOnly = _pipeScriptEngine.IsRunning;
    }

    private void OnStarted()
    {
        InvokeIfRequired(() =>
        {
            UpdateButtons();
            UpdateTitle("Running");
        });
    }

    private void OnFinished()
    {
        InvokeIfRequired(() =>
        {
            _isPaused = false;
            UpdateButtons();
            WriteLine("=== Script finished ===");
            UpdateTitle("Finished");
        });
    }

    private void OnStopped()
    {
        InvokeIfRequired(() =>
        {
            _isPaused = false;
            buttonPauseResume.Text = "Возобновить";
            UpdateButtons();
            WriteLine("=== Script stopped ===");
            UpdateTitle("Stopped");
        });
    }

    private void OnError(string message)
    {
        InvokeIfRequired(() =>
        {
            _isPaused = false;
            buttonPauseResume.Text = "Возобновить";
            UpdateButtons();
            WriteLine("=== ERROR ===");
            WriteLine(message);
            UpdateTitle("Error");
        });
    }

    #endregion

    private void buttonStart_Click(object sender, EventArgs e)
    {
        _pipeScriptEngine.Start(richTextBoxCode.Text);
    }

    private void buttonStop_Click(object sender, EventArgs e)
    {
        _pipeScriptEngine.Stop();
    }

    private void buttonPauseResume_Click(object sender, EventArgs e)
    {
        if (_isPaused)
        {
            _debugger.Resume();
            _isPaused = false;
            buttonPauseResume.Text = "Пауза";
            UpdateTitle("Running");
        }
        else
        {
            _debugger.Pause();
            _isPaused = true;
            buttonPauseResume.Text = "Возобновить";
            UpdateTitle("Paused");
        }

        UpdateButtons();
    }

    private void buttonStep_Click(object sender, EventArgs e)
    {
        _debugger.Step();
    }

    private void buttonStepOver_Click(object sender, EventArgs e)
    {
        _debugger.StepOver();
    }

    private void InvokeIfRequired(Action action)
    {
        if (InvokeRequired)
        {
            BeginInvoke(action);
        }
        else
        {
            action();
        }
    }

    private void UpdateTitle(string status)
    {
        InvokeIfRequired(() => Text = $"{_scriptName} [{status}]");
    }

    #region  IScriptHost


    public void WriteLine(string text)
    {
        Write(text + Environment.NewLine);
    }

    public void Write(string text)
    {
        InvokeIfRequired(() =>
        {
            _richTextBoxNoSmoothScrollOutput.AppendText(text);
            _richTextBoxNoSmoothScrollOutput.SelectionStart = _richTextBoxNoSmoothScrollOutput.TextLength;
            _richTextBoxNoSmoothScrollOutput.ScrollToCaret();
        });
    }

    #endregion
}