import React, { useState } from 'react';
import { TransformWrapper, TransformComponent } from "react-zoom-pan-pinch";
import { type FloorData, type Point } from '../Types/MapType'; 
import BackgroundM from './BackgroundM';
import BackgroundT from './BackgroundT';
import { Icon } from '@iconify/react';
import { RoomType } from '../Types/MapType';

const roomColors: Record<number, { bg: string; hover: string; border: string }> = {
    [RoomType.Classroom]: { 
        bg: "rgba(148, 163, 184, 0.2)", 
        hover: "rgba(148, 163, 184, 0.5)", 
        border: "#64748b" 
    },
    [RoomType.Toilets]: { 
        bg: "rgba(37, 99, 235, 0.2)", 
        hover: "rgba(37, 99, 235, 0.5)", 
        border: "#2563eb" 
    },
    [RoomType.Office]: { 
        bg: "rgba(234, 179, 8, 0.2)", 
        hover: "rgba(234, 179, 8, 0.5)", 
        border: "#ca8a04" 
    },
};

const defaultColor = { 
    bg: "rgba(200, 200, 200, 0.1)", 
    hover: "rgba(200, 200, 200, 0.3)", 
    border: "#999" 
};

interface InteractiveMapProps {
    floors: FloorData[];
    activeFloorId: number;
    buildingId: number;
}

const InteractiveMap: React.FC<InteractiveMapProps> = ({ floors, activeFloorId, buildingId }) => {
    const [selectedPoint, setSelectedPoint] = useState<Point | null>(null);
    const [zoomLevel, setZoomLevel] = useState(1);

    const activeFloorData = floors.find(f => f.floorId === activeFloorId) || floors[0];

    const backgroundMap: Record<number, React.FC<{ zoomLevel: number }>> = {
        1: BackgroundM,
        2: BackgroundT,
    };

    const SelectedBackground = backgroundMap[buildingId] || BackgroundM;

    return (
        <div style={{ width: '100vw', height: '100vh', position: 'relative'}}>
            <TransformWrapper
                onTransform={(ref) => setZoomLevel(ref.state.scale)}
                initialScale={0.85} 
                minScale={0.75}
                centerOnInit={true}
                limitToBounds={true}
            >
                <TransformComponent 
                    wrapperStyle={{ width: '100vw', height: '100vh', backgroundColor: '#EAEAEA' }} 
                    contentStyle={{ width: '130vw', height: '130vh' }}
                >
                    <svg 
                        viewBox="0 0 540 900"
                        preserveAspectRatio="xMidYMid slice"
                        style={{ width: '100%', height: '100%', display: 'block' }}
                    >
                        <SelectedBackground zoomLevel={zoomLevel} />

                        <image 
                            href={activeFloorData.mapImageUrl} 
                            x="0" y="0" width="540" height="900" 
                        />

                        {activeFloorData.rooms.map(room => {
                            const colors = roomColors[room.type] || defaultColor;
                            
                            return (
                                <React.Fragment key={room.roomId}>
                                    <path 
                                        d={room.svgData}
                                        fill={colors.bg} 
                                        stroke={colors.border}            
                                        strokeWidth={1 / zoomLevel} 
                                        onClick={() => console.log("Kliknuto na:", room.label)}
                                        style={{ 
                                            cursor: 'pointer', 
                                            transition: 'fill 0.2s',
                                            pointerEvents: 'all' 
                                        }}
                                        onMouseEnter={(e) => (e.currentTarget.style.fill = colors.hover)}
                                        onMouseLeave={(e) => (e.currentTarget.style.fill = colors.bg)}
                                    />
                                    
                                    {zoomLevel > 1.5 && room.coordinateX != null && room.coordinateY != null && (
                                        room.icon ? (
                                            <foreignObject
                                                x={room.coordinateX - (10 / zoomLevel)}
                                                y={room.coordinateY - (10 / zoomLevel)}
                                                width={20 / zoomLevel}
                                                height={20 / zoomLevel}
                                                style={{ pointerEvents: 'none' }}
                                            >
                                                <div style={{ 
                                                    display: 'flex', 
                                                    justifyContent: 'center', 
                                                    alignItems: 'center', 
                                                    width: '100%', 
                                                    height: '100%' 
                                                }}>
                                                    <Icon icon={room.icon} style={{ width: '100%', height: '100%', color: '#333' }} />
                                                </div>
                                            </foreignObject>
                                        ) : (
                                            room.label && (
                                                <text
                                                    x={room.coordinateX}
                                                    y={room.coordinateY}
                                                    textAnchor="middle"
                                                    dominantBaseline="middle"
                                                    style={{ 
                                                        fontSize: `${12 / zoomLevel}px`, 
                                                        fill: '#333', 
                                                        fontWeight: 'bold',
                                                        pointerEvents: 'none', 
                                                        userSelect: 'none'
                                                    }}
                                                >
                                                    {room.label}
                                                </text>
                                            )
                                        )
                                    )}
                                    
                                    {room.points?.map(point => (
                                        <circle 
                                            key={point.pointId}
                                            cx={point.labelX}
                                            cy={point.labelY}
                                            r={6 / zoomLevel}
                                            fill="#ff0000"
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                setSelectedPoint(point);
                                            }}
                                            style={{ cursor: 'pointer' }}
                                        />
                                    ))}
                                </React.Fragment>
                            );
                        })}
                    </svg>
                </TransformComponent>
            </TransformWrapper>
        </div>
    );
};

export default InteractiveMap;