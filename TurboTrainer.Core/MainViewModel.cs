﻿using ReactiveUI;
using System.Reactive.Concurrency;
using System;
using System.Reactive.Linq;

namespace TurboTrainer.Core
{
    public class MainViewModel : ReactiveObject
    {
        private GpxPoint currentPoint;
        private readonly ReactiveCommand loadGpxDataCommand = new ReactiveCommand();

        public MainViewModel(IScheduler backgroundScheduler, IScheduler mainThreadScheduler, IFileChooserUi fileChooser)
        {
            loadGpxDataCommand.RegisterAsyncAction(_ =>
	                              {
                                      using (var stream = fileChooser.ChooseFile())
									  {
										  var reader = new GpxReader(stream);
										  reader.Points.Replay(backgroundScheduler)
													   .ObserveOn(mainThreadScheduler)
													   .Subscribe(x => CurrentPoint = x);
									  }
	                              });
        }

        public GpxPoint CurrentPoint
        {
            get { return currentPoint; }
            private set { this.RaiseAndSetIfChanged(ref currentPoint, value); }
        }

        public ReactiveCommand LoadGpxDataCommand { get { return loadGpxDataCommand; } }
    }
}
