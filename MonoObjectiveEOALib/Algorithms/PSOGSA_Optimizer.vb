''' <summary>
''' Implemented based on : PSOGSA source code v3.0, Generated by SeyedAli Mirjalili, 2011. 
''' Adopted from: S. Mirjalili, S.Z. Mohd Hashim, A New Hybrid PSOGSA 
''' Algorithm for Function Optimization, in IEEE International Conference 
''' on Computer and Information Application?ICCIA 2010), China, 2010, pp. 374-377.
''' </summary>
Public Class PSOGSA_Optimizer
    Inherits EvolutionaryAlgoBase

    Public Sub New()
    End Sub

    Public Sub New(populationSize As Integer, searchSpaceDimension As Integer, searchSpaceIntervals As List(Of Interval))
        PopulationSize_N = populationSize
        Dimensions_D = searchSpaceDimension
        SearchIntervals = searchSpaceIntervals
    End Sub

    Public Sub New(populationSize As Integer, searchSpaceDimension As Integer, searchSpaceIntervals As List(Of Interval), Go As Double, alphaG As Double, C_1 As Double, C_2 As Double)
        PopulationSize_N = populationSize
        Dimensions_D = searchSpaceDimension
        SearchIntervals = searchSpaceIntervals
        G0 = Go
        Alpha = alphaG
        C1 = C_1
        C2 = C_2
    End Sub

    Public Sub New(populationSize As Integer, searchSpaceDimension As Integer, searchSpaceIntervals As List(Of Interval), maxIterationsNbr As Integer)
        PopulationSize_N = populationSize
        Dimensions_D = searchSpaceDimension
        SearchIntervals = searchSpaceIntervals
        MaxIterations = maxIterationsNbr
    End Sub

    Public Sub New(populationSize As Integer, searchSpaceDimension As Integer, searchSpaceIntervals As List(Of Interval), maxIterationsNbr As Integer, Go As Double, alphaG As Double, C_1 As Double, C_2 As Double)
        PopulationSize_N = populationSize
        Dimensions_D = searchSpaceDimension
        SearchIntervals = searchSpaceIntervals
        MaxIterations = maxIterationsNbr
        G0 = Go
        Alpha = alphaG
        C1 = C_1
        C2 = C_2
    End Sub

    Public Overrides ReadOnly Property AlgorithmName As Object
        Get
            Return "PSOGSA"
        End Get
    End Property

    Public Overrides ReadOnly Property AlgorithmFullName As Object
        Get
            Return "Particle Swarm Optimization - Gravitational Search Algorithm"
        End Get
    End Property

    Public Overrides ReadOnly Property BestSolution As Double()
        Get
            Return gBest
        End Get
    End Property

    Dim _BestChart As List(Of Double)
    Public Overrides ReadOnly Property BestChart As List(Of Double)
        Get
            Return _BestChart
        End Get
    End Property

    Dim _WorstChart As List(Of Double)
    Public Overrides ReadOnly Property WorstChart As List(Of Double)
        Get
            Return _WorstChart
        End Get
    End Property

    Dim _MeanChart As List(Of Double)
    Public Overrides ReadOnly Property MeanChart As List(Of Double)
        Get
            Return _MeanChart
        End Get
    End Property


    Public Overrides ReadOnly Property Solution_Fitness As Dictionary(Of String, Double)
        Get
            Throw New NotImplementedException()
        End Get
    End Property
    Public Overrides ReadOnly Property CurrentBestFitness As Double
        Get
            Return gBestScore
        End Get
    End Property

    Public Overrides Sub RunEpoch()

        G = G0 * Math.Exp((-1 * Alpha * (CurrentIteration - 1)) / MaxIterations)

        For i = 0 To N
            mass(i) = 0
            For j = 0 To D
                force(i, j) = 0
                acceleration(i, j) = 0
            Next
        Next

        'Space_Bound(Population)

        For i = 0 To N
            '' simple bounds/limits
            For j = 0 To D
                If Population(i)(j) < SearchIntervals(j).Min_Value Then
                    Population(i)(j) = SearchIntervals(j).Min_Value '(SearchIntervals(j).Max_Value - SearchIntervals(j).Min_Value) * RandomGenerator.NextDouble() + SearchIntervals(j).Min_Value
                End If

                If Population(i)(j) > SearchIntervals(j).Max_Value Then
                    Population(i)(j) = SearchIntervals(j).Max_Value '(SearchIntervals(j).Max_Value - SearchIntervals(j).Min_Value) * RandomGenerator.NextDouble() + SearchIntervals(j).Min_Value
                End If
            Next

            ''Evaluate the population    
            fitness = 0R
            ComputeObjectiveFunction(Population(i), fitness)
            current_fitness(i) = fitness

            If (pBestScore(i) > fitness) Then
                pBestScore(i) = fitness
                For j = 0 To D
                    pBest(i, j) = current_fitness(i)
                Next
            End If

            If (gBestScore > fitness) Then
                gBestScore = fitness
                For j = 0 To D
                    gBest(j) = Population(i)(j)
                Next
            End If
        Next

        best = current_fitness.Min
        worst = current_fitness.Max
        ''_________________________________________________________
        _BestChart.Add(gBestScore)
        _MeanChart.Add((current_fitness.Sum / N))
        _WorstChart.Add(current_fitness.Max) 'For minimisation only
        ''CurrentBestFitness = gBestScore
        ''---------------------------------------------------------

        ''For pp = 0 To N
        ''    If current_fitness(pp) = best Then
        ''        bestIndex = pp
        ''        Exit For
        ''    End If
        ''Next

        ''Calculate Mass
        For i = 0 To N
            mass(i) = (current_fitness(i) - (0.99 * worst)) / (best - worst)
        Next

        For i = 0 To N
            mass(i) = (mass(i) * 5) / mass.Sum()
        Next

        ''Force update

        For i = 0 To N
            For j = 0 To D
                For k = 0 To N
                    If (Population(k)(j) <> Population(i)(j)) Then
                        ''Equation (3)
                        force(i, j) = force(i, j) + ((RandomGenerator.NextDouble() * G * mass(k) * mass(i) * (Population(k)(j) - Population(i)(j))) / Math.Abs(Population(k)(j) - Population(i)(j)))
                    End If
                Next
            Next
        Next

        ''Accelations & Velocities  UPDATE

        For i = 0 To N
            For j = 0 To D
                If mass(i) <> 0 Then
                    ''%Equation (6)
                    acceleration(i, j) = force(i, j) / mass(i)
                End If
            Next
        Next

        For i = 0 To N
            For j = 0 To D
                ''Equation(9)
                velocity(i, j) = (RandomGenerator.NextDouble() * velocity(i, j)) + (C1 * RandomGenerator.NextDouble() * acceleration(i, j)) + (C2 * RandomGenerator.NextDouble() * (gBest(j) - Population(i)(j)))
            Next
        Next

        '' positions   UPDATE
        '' Equation(10)
        For i = 0 To N
            For j = 0 To D
                Population(i)(j) = Population(i)(j) + velocity(i, j)
            Next
        Next

    End Sub

    Public Property G0 As Double = 1
    Public Property Alpha As Double = 23
    Public Property C1 As Double = 0.5 '' C1 in Equation (9)
    Public Property C2 As Double = 1.5 ''C2 in Equation (9)

#Region "Local_Variables"

    Private N, D As Integer
    Private gBest As Double()
    Private G, gBestScore As Double
    Private pBestScore As Double()
    Private pBest As Double(,)
    Private velocity As Double(,)
    Private acceleration As Double(,)
    Private mass As Double()
    Private force As Double(,)
    Private fitness As Double
    Private current_fitness As Double()
    Private best, worst As Double
    Private bestIndex As Integer
    'Private best_fit_position As Double()

    ''-----for SpaceBound()--------
    Dim Tp As Integer()
    Dim Tm As Integer()
    Dim TpTildeTm As Integer()
    Dim value As Integer = 0I
    Dim TmpArray As Double()
    Dim randiDimm As Double()
    '------------------------------


#End Region
    Public Overrides Sub InitializeOptimizer()
        If SearchIntervals.Count < Dimensions_D Then Throw New Exception("Search space intervals must be equal search space dimension.")

        _BestChart = New List(Of Double)
        _MeanChart = New List(Of Double)
        _WorstChart = New List(Of Double)

        D = (Dimensions_D - 1)
        N = (PopulationSize_N - 1)

        gBest = New Double(D) {}
        current_fitness = New Double(N) {}

        If OptimizationType = OptimizationTypeEnum.Minimization Then
            gBestScore = Double.MaxValue
        Else
            gBestScore = Double.MinValue
        End If

        pBestScore = New Double(N) {}
        If OptimizationType = OptimizationTypeEnum.Minimization Then
            For i = 0 To N
                pBestScore(i) = Double.MaxValue
            Next
        Else
            For i = 0 To N
                pBestScore(i) = Double.MinValue
            Next
        End If

        pBest = New Double(N, D) {}

        ''---------------for SpaceBound()-------------
        Tp = New Integer(D) {}
        Tm = New Integer(D) {}
        TpTildeTm = New Integer(D) {}
        TmpArray = New Double(D) {}
        randiDimm = New Double(D) {}


        'Initialize population
        InitializePopulation()

        'initialize velocities
        velocity = New Double(N, D) {}
        For i = 0 To N
            For j = 0 To D
                While (velocity(i, j) = 0)
                    velocity(i, j) = 0.3 * RandomGenerator.NextDouble() * RandomGenerator.Next(-1, 2)
                End While
            Next
        Next

        acceleration = New Double(N, D) {}
        mass = New Double(N) {}
        force = New Double(N, D) {}

        'best_fit_position = New Double(MaxIterations - 1) {}

    End Sub


    Public Sub Space_Bound(ByRef X As Double()())
        ''from matlab site :
        ''https://www.mathworks.com/matlabcentral/answers/311735-hi-i-try-to-convert-this-matlab-code-to-vb-net-or-c-codes-help-me-please
        ''outofrange = X(i, : ) > up | X(i, :) < low;
        ''X(i, outofrange) = rand(1, sum(outofrange)) * (up - low) + low;

        For i As Integer = 0 To Me.N

            For j As Integer = 0 To Me.D
                If X(i)(j) > SearchIntervals.Item(j).Max_Value Then
                    Tp(j) = 1I
                Else
                    Tp(j) = 0I
                End If

                If X(i)(j) < SearchIntervals.Item(j).Min_Value Then
                    Tm(j) = 1I

                Else
                    Tm(j) = 0I
                End If

                value = Tp(j) + Tm(j)

                If value = 0 Then
                    TpTildeTm(j) = 1I
                Else
                    TpTildeTm(j) = 0I
                End If
            Next

            '------------------------------------
            For j As Integer = 0 To Me.D
                TmpArray(j) = X(i)(j) * TpTildeTm(j)
            Next
            '-----------------------------------
            For t = 0 To Me.D
                randiDimm(t) = (RandomGenerator.NextDouble() * (SearchIntervals.Item(t).Max_Value - SearchIntervals.Item(t).Min_Value) + SearchIntervals.Item(t).Min_Value) * (Tp(t) + Tm(t))
            Next

            For t = 0 To Me.D
                X(i)(t) = TmpArray(t) + randiDimm(t)
            Next
        Next
    End Sub



    Public Overrides Sub ComputeObjectiveFunction(positions() As Double, ByRef fitness_Value As Double)
        MyBase.OnObjectiveFunction(positions, fitness_Value)
    End Sub
End Class