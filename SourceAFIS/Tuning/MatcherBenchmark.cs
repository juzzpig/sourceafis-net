using System;
using System.Collections.Generic;
using System.Text;
using SourceAFIS.General;
using SourceAFIS.Extraction.Templates;
using SourceAFIS.Matching;
using SourceAFIS.Tuning.Errors;
using SourceAFIS.Tuning.Reports;

namespace SourceAFIS.Tuning
{
    public sealed class MatcherBenchmark
    {
        public TestDatabase TestDatabase = new TestDatabase();
        public Matcher Matcher = new Matcher();
        public float Timeout = 300;

        public MatcherReport Run()
        {
            MatcherReport report = new MatcherReport();
            report.SetDatabaseCount(TestDatabase.Databases.Count);
            
            BenchmarkTimer prepareTimer = new BenchmarkTimer();
            BenchmarkTimer matchingTimer = new BenchmarkTimer();
            BenchmarkTimer nonmatchingTimer = new BenchmarkTimer();

            for (int databaseIndex = 0; databaseIndex < TestDatabase.Databases.Count; ++databaseIndex)
            {
                TestDatabase.Database database = TestDatabase.Databases[databaseIndex];
                report.ScoreTables[databaseIndex].Initialize(database);
                for (int fingerIndex = 0; fingerIndex < database.Fingers.Count; ++fingerIndex)
                {
                    TestDatabase.Finger finger = database.Fingers[fingerIndex];
                    for (int viewIndex = 0; viewIndex < finger.Views.Count; ++viewIndex)
                    {
                        TestDatabase.View view = finger.Views[viewIndex];
                        RunPrepare(view.Template, prepareTimer);

                        List<Template> matching = new List<Template>();
                        for (int candidateView = 0; candidateView < finger.Views.Count; ++candidateView)
                            if (candidateView != viewIndex)
                                matching.Add(finger.Views[candidateView].Template);
                        report.ScoreTables[databaseIndex].Table[fingerIndex][viewIndex].Matching = RunMatch(matching, matchingTimer);

                        List<Template> nonmatching = new List<Template>();
                        for (int candidateFinger = 0; candidateFinger < database.Fingers.Count; ++candidateFinger)
                            if (candidateFinger != fingerIndex)
                                nonmatching.Add(database.Fingers[candidateFinger].Views[viewIndex].Template);
                        report.ScoreTables[databaseIndex].Table[fingerIndex][viewIndex].NonMatching = RunMatch(nonmatching, nonmatchingTimer);

                        if (prepareTimer.Accumulated.TotalSeconds + matchingTimer.Accumulated.TotalSeconds +
                            nonmatchingTimer.Accumulated.TotalSeconds > Timeout)
                        {
                            throw new TimeoutException("Timeout in matcher");
                        }
                    }
                }
            }

            report.Time.Prepare = (float)prepareTimer.Accumulated.TotalSeconds / TestDatabase.GetFingerprintCount();
            report.Time.Matching = (float)matchingTimer.Accumulated.TotalSeconds / TestDatabase.GetMatchingPairCount();
            report.Time.NonMatching = (float)nonmatchingTimer.Accumulated.TotalSeconds / TestDatabase.GetNonMatchingPairCount();

            report.ComputeStatistics();

            return report;
        }

        void RunPrepare(Template template, BenchmarkTimer timer)
        {
            timer.Start();
            Matcher.Prepare(template);
            timer.Stop();
        }

        float[] RunMatch(List<Template> templates, BenchmarkTimer timer)
        {
            timer.Start();
            float[] scores = Matcher.Match(templates);
            timer.Stop();
            return scores;
        }
    }
}
