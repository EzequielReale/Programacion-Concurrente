VerifyArray(int[] array, int number) {

    int ocurrences = finished = lowLimit = 0, arraySize = array.size(), processes = arraySize / 5; 

    Process FindNumber() [i = 0 to processes-1] { 
        int myLowLimit, myUpLimit, aux_ocurrences = 0;
        <myLowLimit = lowLimit; lowLimit += 5;>
        myUpLimit = myLowLimit + 5;
        for (myLowLimit to myUpLimit) if (array[myLowLimit] == number) aux_ocurrences++;
        <ocurrences += aux_ocurrences;>
        <finished++;>
    }

    <await (finished == processes);>
    print(ocurrences);
}
