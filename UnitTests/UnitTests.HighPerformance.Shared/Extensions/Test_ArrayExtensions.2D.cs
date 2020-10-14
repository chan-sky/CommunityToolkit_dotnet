﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Extensions
{
    public partial class Test_ArrayExtensions
    {
        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_2D_DangerousGetReference_Int()
        {
            int[,] array =
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 }
            };

            ref int r0 = ref array.DangerousGetReference();
            ref int r1 = ref array[0, 0];

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_2D_DangerousGetReference_String()
        {
            string[,] array =
            {
                { "a", "bb", "ccc" },
                { "dddd", "eeeee", "ffffff" }
            };

            ref string r0 = ref array.DangerousGetReference();
            ref string r1 = ref array[0, 0];

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_2D_DangerousGetReferenceAt_Zero()
        {
            int[,] array =
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 }
            };

            ref int r0 = ref array.DangerousGetReferenceAt(0, 0);
            ref int r1 = ref array[0, 0];

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_2D_DangerousGetReferenceAt_Index()
        {
            int[,] array =
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 }
            };

            ref int r0 = ref array.DangerousGetReferenceAt(1, 3);
            ref int r1 = ref array[1, 3];

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_2D_AsSpan2DAndFillArrayMid()
        {
            bool[,] test = new bool[4, 5];

            test.AsSpan2D(1, 1, 2, 3).Fill(true);

            var expected = new[,]
            {
                { false, false, false, false, false },
                { false,  true,  true,  true, false },
                { false,  true,  true,  true, false },
                { false, false, false, false, false },
            };

            CollectionAssert.AreEqual(expected, test);
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_2D_AsSpan2DAndFillArrayTwice()
        {
            bool[,] test = new bool[4, 5];

            test.AsSpan2D(0, 0, 2, 1).Fill(true);
            test.AsSpan2D(1, 3, 2, 2).Fill(true);

            var expected = new[,]
            {
                { true,  false, false, false, false },
                { true,  false, false,  true,  true },
                { false, false, false,  true,  true },
                { false, false, false, false, false },
            };

            CollectionAssert.AreEqual(expected, test);
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_2D_AsSpan2DAndFillArrayBottomEdgeBoundary()
        {
            bool[,] test = new bool[4, 5];

            test.AsSpan2D(1, 2, 3, 2).Fill(true);

            var expected = new[,]
            {
                { false, false, false, false, false },
                { false, false,  true,  true, false },
                { false, false,  true,  true, false },
                { false, false,  true,  true, false },
            };

            CollectionAssert.AreEqual(expected, test);
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_2D_AsSpan2DAndFillArrayBottomRightCornerBoundary()
        {
            bool[,] test = new bool[5, 4];

            test.AsSpan2D(3, 2, 2, 2).Fill(true);

            var expected = new[,]
            {
                { false, false, false, false },
                { false, false, false, false },
                { false, false, false, false },
                { false, false,  true,  true },
                { false, false,  true,  true },
            };

            CollectionAssert.AreEqual(expected, test);
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1312", Justification = "Dummy loop variable")]
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1501", Justification = "Empty test loop")]
        public void Test_ArrayExtensions_2D_GetRow_Rectangle()
        {
            int[,] array =
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 }
            };

            int j = 0;
            foreach (ref int value in array.GetRow(1))
            {
                Assert.IsTrue(Unsafe.AreSame(ref value, ref array[1, j++]));
            }

            CollectionAssert.AreEqual(array.GetRow(1).ToArray(), new[] { 5, 6, 7, 8 });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetRow(-1));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetRow(20));
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_2D_GetRow_Empty()
        {
            int[,] array = new int[0, 0];

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetRow(0).ToArray());
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1312", Justification = "Dummy loop variable")]
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1501", Justification = "Empty test loop")]
        public void Test_ArrayExtensions_2D_GetColumn_Rectangle()
        {
            int[,] array =
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 }
            };

            int i = 0;
            foreach (ref int value in array.GetColumn(1))
            {
                Assert.IsTrue(Unsafe.AreSame(ref value, ref array[i++, 1]));
            }

            CollectionAssert.AreEqual(array.GetColumn(1).ToArray(), new[] { 2, 6, 10 });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetColumn(-1));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetColumn(20));
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_2D_GetColumn_Empty()
        {
            int[,] array = new int[0, 0];

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetColumn(0).ToArray());
        }

#if NETCOREAPP3_1
        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_2D_AsSpan_Empty()
        {
            int[,] array = new int[0, 0];

            Span<int> span = array.AsSpan();

            Assert.AreEqual(span.Length, array.Length);
            Assert.IsTrue(span.IsEmpty);
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_2D_AsSpan_Populated()
        {
            int[,] array =
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 }
            };

            Span<int> span = array.AsSpan();

            Assert.AreEqual(span.Length, array.Length);

            ref int r0 = ref array[0, 0];
            ref int r1 = ref span[0];

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }
#endif
    }
}
