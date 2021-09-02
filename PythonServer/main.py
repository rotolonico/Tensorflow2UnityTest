import zmq
import numpy as np
from keras.models import load_model

model = load_model('model.h5')

context = zmq.Context()
socket = context.socket(zmq.REP)
socket.bind("tcp://*:5555")

while True:
    bytes_received = socket.recv(3136)
    array_received = np.frombuffer(bytes_received, dtype=np.float32).reshape(28,28)

    pred = model.predict(array_received.reshape(1,784))

    bytes_to_send = pred.tobytes()
    socket.send(bytes_to_send)
