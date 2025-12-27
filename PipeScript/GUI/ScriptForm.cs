using System;
using System.Windows.Forms;
using PipeScript.API;

namespace PipeScript.GUI;

public sealed partial class ScriptForm : Form, IScriptHost
{
    private readonly PipeScriptEngine _pipeScriptEngine;
    private readonly string _scriptName;
    private readonly string _code;

    private bool _isPaused;

    public ScriptForm(string scriptName, string code)
    {
        InitializeComponent();
        Text = scriptName;
        _pipeScriptEngine = new PipeScriptEngine(scriptName)
        {
            Context =
            {
                Host = new WinFormsHost(this)
            }
        };
        _scriptName = scriptName;
        _code = code;

        _pipeScriptEngine.Started += OnStarted;
        _pipeScriptEngine.Paused += OnPaused;
        _pipeScriptEngine.Resumed += OnResumed;
        _pipeScriptEngine.Finished += OnFinished;
        _pipeScriptEngine.Stopped += OnStopped;
        _pipeScriptEngine.Error += OnError;
    }

    #region Событие движка

    private void OnStarted()
    {
        InvokeIfRequired(() =>
        {
            buttonStop.Enabled = true;
            buttonStep.Enabled = _pipeScriptEngine.IsRunning && _isPaused;
            UpdateTitle("Running");
        });
    }

    private void OnPaused()
    {
        InvokeIfRequired(() =>
        {
            _isPaused = true;
            buttonPauseResume.Text = "Возобновить";
            buttonStep.Enabled = _pipeScriptEngine.IsRunning && _isPaused;
            UpdateTitle("Paused");
        });
    }

    private void OnResumed()
    {
        InvokeIfRequired(() =>
        {
            _isPaused = false;
            buttonPauseResume.Text = "Пауза";
            buttonStep.Enabled = _pipeScriptEngine.IsRunning && _isPaused;
            UpdateTitle("Running");
        });
    }

    private void OnFinished()
    {
        InvokeIfRequired(() =>
        {
            _isPaused = false;
            buttonStop.Enabled = false;
            buttonPauseResume.Text = "Пауза";
            buttonStep.Enabled = _pipeScriptEngine.IsRunning && _isPaused;
            WriteLine("=== Script finished ===");
            UpdateTitle("Finished");
        });
    }

    private void OnStopped()
    {
        InvokeIfRequired(() =>
        {
            _isPaused = false;
            buttonStop.Enabled = false;
            buttonPauseResume.Text = "Пауза";
            buttonStep.Enabled = _pipeScriptEngine.IsRunning && _isPaused;
            WriteLine("=== Script stopped ===");
            UpdateTitle("Stopped");
        });
    }

    private void OnError(string message)
    {
        InvokeIfRequired(() =>
        {
            _isPaused = false;
            buttonPauseResume.Text = "Пауза";
            buttonStop.Enabled = false;
            WriteLine("=== ERROR ===");
            WriteLine(message);

            UpdateTitle("Error");
        });
    }


    #endregion

    private void buttonStart_Click(object sender, EventArgs e)
    {
        _pipeScriptEngine.Start(_code);
    }

    private void buttonStop_Click(object sender, EventArgs e)
    {
        _pipeScriptEngine.Stop();
    }

    private void buttonPauseResume_Click(object sender, EventArgs e)
    {
        if (_isPaused)
        {
            _pipeScriptEngine.Resume();
        }
        else
        {
            _pipeScriptEngine.Pause();
        }
    }

    private void buttonStep_Click(object sender, EventArgs e)
    {
        _pipeScriptEngine.Step();
    }

    public void WriteLine(string text)
    {
        InvokeIfRequired(() =>
        {
            richTextBoxOutput.AppendText(text + Environment.NewLine);
        });
    }

    public void Write(string text)
    {
        InvokeIfRequired(() =>
        {
            richTextBoxOutput.AppendText(text);
        });
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
}
