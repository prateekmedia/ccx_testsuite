﻿namespace CCExtractorTester.Enums
{
    /// <summary>
    /// Represents the possible comparisons that the program supports.
    /// </summary>
    public enum CompareType
    {
        /// <summary>
        /// Regular diff (Linux only), using the diff command.
        /// </summary>
        Diff,
        /// <summary>
        /// Generates HTML viewable diffs for two files (showing the full file).
        /// </summary>
        Diffplex,
        /// <summary>
        /// Generates HTML viewable diffs for two files (showing changes only).
        /// </summary>
        Diffplexreduced
    }
}