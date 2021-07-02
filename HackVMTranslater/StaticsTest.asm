// Bootstrap code
@256
D=A
@0
M=D

//call Sys.init 0
// push retAddrLabel
@Sys.init$ret.0
D=A
@SP
A=M
M=D
@SP
M=M+1

// push LCL
@LCL
D=M
@SP
A=M
M=D
@SP
M=M+1

// push ARG
@ARG
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THIS
@THIS
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THAT
@THAT
D=M
@SP
A=M
M=D
@SP
M=M+1

// ARG = SP-5-nArgs
@SP
D=M
@5
D=D-A
@0
D=D-A
@ARG
M=D
// LCL = SP
@SP
D=M
@LCL
M=D
@Sys.init
0;JMP
(Sys.init$ret.0)

//Class1.set 0
(Class1.set)

@0
D=A
@SP
M=D+M

// C_PUSH argument 0
@ARG
D=M
@0
A=D+A
D=M

@SP
A=M
M=D
@SP
M=M+1


// C_POP static 0
@SP
M=M-1
A=M
D=M

@Class1.0
M=D


// C_PUSH argument 1
@ARG
D=M
@1
A=D+A
D=M

@SP
A=M
M=D
@SP
M=M+1


// C_POP static 1
@SP
M=M-1
A=M
D=M

@Class1.1
M=D


// C_PUSH constant 0
@0
D=A

@SP
A=M
M=D
@SP
M=M+1


// return
@LCL
D=M
@R14
M=D
// retAddr = *(endFrame-5)
@5
D=A
@LCL
D=M-D
A=D
D=M
@R13
M=D
// *ARG=pop()
@SP
M=M-1
A=M
D=M

@ARG
A=M
M=D
// SP=ARG+1
@ARG
D=M
@1
D=D+A
@SP
M=D
//THAT = *(endFrame-1)
@R14
D=M
@1
D=D-A
A=D
D=M
@THAT
M=D
//THIS = *(endFrame-2)
@R14
D=M
@2
D=D-A
A=D
D=M
@THIS
M=D
//ARG = *(endFrame-3)
@R14
D=M
@3
D=D-A
A=D
D=M
@ARG
M=D
//LCL = *(endFrame-4)
@R14
D=M
@4
D=D-A
A=D
D=M
@LCL
M=D
// goto retAddr
@R13
A=M
0;JMP
@SP
M=M-1
A=M
D=M


//Class1.get 0
(Class1.get)

@0
D=A
@SP
M=D+M

// C_PUSH static 0
@Class1.0
D=M

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH static 1
@Class1.1
D=M

@SP
A=M
M=D
@SP
M=M+1


// sub
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M-D
@SP
A=M
M=D
@SP
M=M+1


// return
@LCL
D=M
@R14
M=D
// retAddr = *(endFrame-5)
@5
D=A
@LCL
D=M-D
A=D
D=M
@R13
M=D
// *ARG=pop()
@SP
M=M-1
A=M
D=M

@ARG
A=M
M=D
// SP=ARG+1
@ARG
D=M
@1
D=D+A
@SP
M=D
//THAT = *(endFrame-1)
@R14
D=M
@1
D=D-A
A=D
D=M
@THAT
M=D
//THIS = *(endFrame-2)
@R14
D=M
@2
D=D-A
A=D
D=M
@THIS
M=D
//ARG = *(endFrame-3)
@R14
D=M
@3
D=D-A
A=D
D=M
@ARG
M=D
//LCL = *(endFrame-4)
@R14
D=M
@4
D=D-A
A=D
D=M
@LCL
M=D
// goto retAddr
@R13
A=M
0;JMP
@SP
M=M-1
A=M
D=M


//Class2.set 0
(Class2.set)

@0
D=A
@SP
M=D+M

// C_PUSH argument 0
@ARG
D=M
@0
A=D+A
D=M

@SP
A=M
M=D
@SP
M=M+1


// C_POP static 0
@SP
M=M-1
A=M
D=M

@Class2.0
M=D


// C_PUSH argument 1
@ARG
D=M
@1
A=D+A
D=M

@SP
A=M
M=D
@SP
M=M+1


// C_POP static 1
@SP
M=M-1
A=M
D=M

@Class2.1
M=D


// C_PUSH constant 0
@0
D=A

@SP
A=M
M=D
@SP
M=M+1


// return
@LCL
D=M
@R14
M=D
// retAddr = *(endFrame-5)
@5
D=A
@LCL
D=M-D
A=D
D=M
@R13
M=D
// *ARG=pop()
@SP
M=M-1
A=M
D=M

@ARG
A=M
M=D
// SP=ARG+1
@ARG
D=M
@1
D=D+A
@SP
M=D
//THAT = *(endFrame-1)
@R14
D=M
@1
D=D-A
A=D
D=M
@THAT
M=D
//THIS = *(endFrame-2)
@R14
D=M
@2
D=D-A
A=D
D=M
@THIS
M=D
//ARG = *(endFrame-3)
@R14
D=M
@3
D=D-A
A=D
D=M
@ARG
M=D
//LCL = *(endFrame-4)
@R14
D=M
@4
D=D-A
A=D
D=M
@LCL
M=D
// goto retAddr
@R13
A=M
0;JMP
@SP
M=M-1
A=M
D=M


//Class2.get 0
(Class2.get)

@0
D=A
@SP
M=D+M

// C_PUSH static 0
@Class2.0
D=M

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH static 1
@Class2.1
D=M

@SP
A=M
M=D
@SP
M=M+1


// sub
@SP
M=M-1
A=M
D=M
@SP
M=M-1
A=M
D=M-D
@SP
A=M
M=D
@SP
M=M+1


// return
@LCL
D=M
@R14
M=D
// retAddr = *(endFrame-5)
@5
D=A
@LCL
D=M-D
A=D
D=M
@R13
M=D
// *ARG=pop()
@SP
M=M-1
A=M
D=M

@ARG
A=M
M=D
// SP=ARG+1
@ARG
D=M
@1
D=D+A
@SP
M=D
//THAT = *(endFrame-1)
@R14
D=M
@1
D=D-A
A=D
D=M
@THAT
M=D
//THIS = *(endFrame-2)
@R14
D=M
@2
D=D-A
A=D
D=M
@THIS
M=D
//ARG = *(endFrame-3)
@R14
D=M
@3
D=D-A
A=D
D=M
@ARG
M=D
//LCL = *(endFrame-4)
@R14
D=M
@4
D=D-A
A=D
D=M
@LCL
M=D
// goto retAddr
@R13
A=M
0;JMP
@SP
M=M-1
A=M
D=M


//Sys.init 0
(Sys.init)

@0
D=A
@SP
M=D+M

// C_PUSH constant 6
@6
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 8
@8
D=A

@SP
A=M
M=D
@SP
M=M+1


//call Class1.set 2
// push retAddrLabel
@Class1.set$ret.1
D=A
@SP
A=M
M=D
@SP
M=M+1

// push LCL
@LCL
D=M
@SP
A=M
M=D
@SP
M=M+1

// push ARG
@ARG
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THIS
@THIS
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THAT
@THAT
D=M
@SP
A=M
M=D
@SP
M=M+1

// ARG = SP-5-nArgs
@SP
D=M
@5
D=D-A
@2
D=D-A
@ARG
M=D
// LCL = SP
@SP
D=M
@LCL
M=D
@Class1.set
0;JMP
(Class1.set$ret.1)

// C_POP temp 0
@SP
M=M-1
A=M
D=M

@5
M=D


// C_PUSH constant 23
@23
D=A

@SP
A=M
M=D
@SP
M=M+1


// C_PUSH constant 15
@15
D=A

@SP
A=M
M=D
@SP
M=M+1


//call Class2.set 2
// push retAddrLabel
@Class2.set$ret.2
D=A
@SP
A=M
M=D
@SP
M=M+1

// push LCL
@LCL
D=M
@SP
A=M
M=D
@SP
M=M+1

// push ARG
@ARG
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THIS
@THIS
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THAT
@THAT
D=M
@SP
A=M
M=D
@SP
M=M+1

// ARG = SP-5-nArgs
@SP
D=M
@5
D=D-A
@2
D=D-A
@ARG
M=D
// LCL = SP
@SP
D=M
@LCL
M=D
@Class2.set
0;JMP
(Class2.set$ret.2)

// C_POP temp 0
@SP
M=M-1
A=M
D=M

@5
M=D


//call Class1.get 0
// push retAddrLabel
@Class1.get$ret.3
D=A
@SP
A=M
M=D
@SP
M=M+1

// push LCL
@LCL
D=M
@SP
A=M
M=D
@SP
M=M+1

// push ARG
@ARG
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THIS
@THIS
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THAT
@THAT
D=M
@SP
A=M
M=D
@SP
M=M+1

// ARG = SP-5-nArgs
@SP
D=M
@5
D=D-A
@0
D=D-A
@ARG
M=D
// LCL = SP
@SP
D=M
@LCL
M=D
@Class1.get
0;JMP
(Class1.get$ret.3)

//call Class2.get 0
// push retAddrLabel
@Class2.get$ret.4
D=A
@SP
A=M
M=D
@SP
M=M+1

// push LCL
@LCL
D=M
@SP
A=M
M=D
@SP
M=M+1

// push ARG
@ARG
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THIS
@THIS
D=M
@SP
A=M
M=D
@SP
M=M+1

// push THAT
@THAT
D=M
@SP
A=M
M=D
@SP
M=M+1

// ARG = SP-5-nArgs
@SP
D=M
@5
D=D-A
@0
D=D-A
@ARG
M=D
// LCL = SP
@SP
D=M
@LCL
M=D
@Class2.get
0;JMP
(Class2.get$ret.4)

(Sys.init.WHILE)
@Sys.init.WHILE
0;JMP

(END)
@END
0;JMP

