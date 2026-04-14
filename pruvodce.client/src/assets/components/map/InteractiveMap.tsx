import React, { useState } from 'react';
import { TransformWrapper, TransformComponent } from "react-zoom-pan-pinch";
import Room from './Room';

interface RoomData {
  id: string;
  pathData: string;
  label: string;
  labelCoords: { x: number; y: number };
}

interface FloorConfiguration {
  outline: string;
  rooms: RoomData[];
}

const InteractiveMap: React.FC = () => {
  const [zoomLevel, setZoomLevel] = useState<number>(1);
  const [currentFloor, setCurrentFloor] = useState<1 | 2>(1);
  const [selectedRoom, setSelectedRoom] = useState<string | null>(null);

  {/* pak databaze */}
  const floors: Record<number, FloorConfiguration> = {
    1: {
      outline: "M536.815 126H527.994V503H528.083V573H91.083V503H91.1719V371H203.277V452H384.788V302H412.777V281H427.806V263H321.605V241H228.431V260H198.374V241H95.1797V111H321.605V97H432.815V78H536.815V126Z",
      rooms: [
        {
          id: "B110",
          pathData: "M 100 130 H 180 V 230 H 100 Z",
          label: "A310",
          labelCoords: { x: 140, y: 185 }
        }
      ]
    },
    2: {
      outline: "M536.815 126H527.994V503H528.083V573H91.083V503H91.1719V371H203.277V452H384.788V302H412.777V281H427.806V263H321.605V241H228.431V260H198.374V241H95.1797V111H321.605V97H432.815V78H536.815V126Z",
      rooms: [
        {
          id: "B211",
          pathData: "M 330 130 H 420 V 230 H 330 Z",
          label: "B401",
          labelCoords: { x: 375, y: 185 }
        }
      ]
    }
  };

  const sharedBackground = "M190.358 78H130.245V65H67.127V223H0.126953V65H0V0H190.358V78Z";

  const handleRoomClick = (id: string): void => {
    setSelectedRoom(id);
    console.log(`Kliknuto na: ${id}`);
  };

  return (
    <div style={{ width: '100vw', height: '100vh', display: 'flex', flexDirection: 'column', background: '#dad5d5', overflow: 'hidden', position: 'relative' }}>
      
      <div style={{ position: 'absolute', top: 20, left: 20, zIndex: 100, display: 'flex', gap: '5px' }}>
        <button onClick={() => setCurrentFloor(1)}>Patro 1</button>
        <button onClick={() => setCurrentFloor(2)}>Patro 2</button>
        <span style={{ marginLeft: '10px', fontWeight: 'bold' }}>Aktualni aptro: {currentFloor}</span>
      </div>

      <TransformWrapper
        onTransform={(ref) => setZoomLevel(ref.state.scale)}
        initialScale={1}
        minScale={0.2}
        maxScale={8}
        centerOnInit={true}
      >
        <TransformComponent 
          wrapperStyle={{ width: '100vw', height: '100vh' }}
          contentStyle={{ display: 'flex', alignItems: 'center', justifyContent: 'center', width: '100%', height: '100%' }}
        >
          <svg
            viewBox="0 0 540 600" 
            style={{ width: '80%', height: 'auto', maxHeight: '90%', background: 'white', display: 'block' }}
          >
            {/* zem */}
            <path 
              d={sharedBackground}
              fill="#e0e0e0" 
              stroke="#999" 
              strokeWidth="1"
              vectorEffect="non-scaling-stroke"
            />
            {/* patro */}
            <path 
              d={floors[currentFloor].outline}
              fill="#f9f9f9" 
              stroke="black" 
              strokeWidth="1.5"
              vectorEffect="non-scaling-stroke"
            />

            {/* učebny */}
            {floors[currentFloor].rooms.map((room) => (
              <Room
                key={room.id}
                id={room.id}
                pathData={room.pathData}
                label={room.label}
                labelCoords={room.labelCoords}
                isVisible={zoomLevel > 1.8}
                onClick={handleRoomClick}
              />
            ))}

            {/* SMAZAT, JEN DOKUD NEMAM NARESLENY PATRA */}
            <text 
              x="20" y="580" 
              fontSize="20" 
              fill="#ccc" 
              style={{ pointerEvents: 'none', fontWeight: 'bold' }}
            >
              LEVEL_{currentFloor}
            </text>
            
          </svg>
        </TransformComponent>
      </TransformWrapper>
    </div>
  );
};

export default InteractiveMap;