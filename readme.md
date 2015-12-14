# K\#

This is just a toy language. The goal is not to create a PL that could eventually be used tp solve business problems. Rather, I just want to have fun writing a silly little compiler. And, hopefully grow from the experience.

## Goals

* Compile an executable (.exe) that will run in the .NET runtime
* Use Klingon words as keywords in K# - *a toy PL is more fun with a gimmick, right?*
* Implement variables and functions

## Resources/Reference

* **http://llvm.org/docs/tutorial/index.html** This is a good reference to study for understanding a simple lexer (tokenizer,) abstract syntax tree, and parser.

* **http://www.llvmpy.org/llvmpy-doc/dev/index.html** If you're not super familiar with C++ and C (like me,) then you might find this be useful. Comparing and contrasting the Python based tutorial with the C++ based tutorial helped me better understand the concepts and catch errors in my interpretation.

* **http://stackoverflow.com/a/2599037/336384** This answer on Stack Overflow shows a very simple way to build an executable that runs on .NET.