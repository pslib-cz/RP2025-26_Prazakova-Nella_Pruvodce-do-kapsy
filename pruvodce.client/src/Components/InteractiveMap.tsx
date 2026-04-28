import React, { useState } from 'react';
import { TransformWrapper, TransformComponent } from "react-zoom-pan-pinch";
import { type FloorData, type Point } from '../Types/MapType'; 
import BackgroundM from './BackgroundM';
import BackgroundT from './BackgroundT';
import { Icon } from '@iconify/react';
import { RoomType } from '../Types/MapType';

const roomColors: Record<number, { bg: string; hover: string; border: string }> = {
    [RoomType.Classroom]: { 
        bg: "url(#gradient-classroom)", 
        hover: "rgba(205, 205, 205, 0.7)", 
        border: "#50555A" 
    },
    [RoomType.Specialized]: { 
        bg: "url(#gradient-specialized)", 
        hover: "rgba(192, 192, 192, 0.7)", 
        border: "#50555A" 
    },
    [RoomType.Office]: { 
        bg: "url(#gradient-office)", 
        hover: "rgba(170, 170, 170, 0.7)", 
        border: "#50555A" 
    },
    [RoomType.Toilets]: { 
        bg: "url(#gradient-wc)", 
        hover: "rgba(181, 198, 212, 0.7)", 
        border: "#50555A" 
    },
    [RoomType.Elevator]: {
        bg: "url(#gradient-elevator)",
        hover: "rgba(107, 112, 116, 0.7)",
        border: "#50555A"
    },
    [RoomType.Other]: {
        bg: "url(#gradient-other)",
        hover: "rgba(200, 200, 200, 0.7)",
        border: "#50555A"
    }
};

interface InteractiveMapProps {
    floors: FloorData[];
    activeFloorId: number;
    buildingId: number;
    className?: string;
}

const InteractiveMap: React.FC<InteractiveMapProps> = ({ floors, activeFloorId, buildingId, className }) => {
    const [selectedPoint, setSelectedPoint] = useState<Point | null>(null);
    const [zoomLevel, setZoomLevel] = useState(1);

    const isDesktop = window.innerWidth >= 1024;

    const activeFloorData = floors.find(f => f.floorId === activeFloorId) || floors[0];

    const backgroundMap: Record<number, React.FC<{ zoomLevel: number }>> = {
        1: BackgroundM,
        2: BackgroundT,
    };

    const SelectedBackground = backgroundMap[buildingId] || BackgroundM;

    return (
        <div className={`map-wrapper ${className || ''}`}>
            <TransformWrapper
                onTransform={(ref) => setZoomLevel(ref.state.scale)}
                initialScale={isDesktop ? 0.6 : 0.95}
                minScale={isDesktop ? 0.5 : 0.8}
                initialPositionX={isDesktop ? 160 : 0}
                initialPositionY={isDesktop ? -120 : 0}
                limitToBounds={true}
            >
                <TransformComponent 
                    wrapperStyle={{ width: '100vw', height: '100vh', backgroundColor: '#EAEAEA' }} 
                    contentStyle={{ width: isDesktop ? '80vw' : '100vw', height: isDesktop ? '220vh' : '100vh' }}
                >
                    <svg 
                        viewBox="0 0 540 900"
                        preserveAspectRatio="xMidYMid slice"
                        style={{ width: '100%', height: '100%', display: 'block' }}
                    >
                        <defs>
                            <radialGradient id="gradient-classroom" cx="0" cy="0" r="1" gradientUnits="userSpaceOnUse" gradientTransform="translate(22 50) rotate(66.2505) scale(54.626 54.626)">
                            <stop stopColor="#E5E4E2"/>
                            <stop offset="1" stopColor="#CDCDCD"/>
                            </radialGradient>
                        </defs>
                        <defs>
                            <radialGradient id="gradient-office" cx="0" cy="0" r="1" gradientTransform="matrix(-9.04412 14.4189 -10.6985 -12.1892 9.15441 18.7297)" gradientUnits="userSpaceOnUse">
                            <stop stopColor="#C6C4C1"/>
                            <stop offset="1" stopColor="#AAAAAA"/>
                            </radialGradient>
                        </defs>
                        <defs>
                            <radialGradient id="gradient-elevator" cx="0" cy="0" r="1" gradientUnits="userSpaceOnUse" gradientTransform="translate(7.5 23) rotate(90) scale(23 7.5)">
                            <stop stopColor="#C58D73"/>
                            <stop offset="1" stopColor="#B87B5E"/>
                            </radialGradient>
                        </defs>
                        <defs>
                            <radialGradient id="gradient-wc" cx="0" cy="0" r="1" gradientUnits="userSpaceOnUse" gradientTransform="translate(10 22.5) rotate(90) scale(31.1538 8.85246)">
                            <stop stopColor="#C4D4E1"/>
                            <stop offset="1" stopColor="#B5C6D4"/>
                            </radialGradient>
                        </defs>
                        <defs>
                            <radialGradient id="gradient-other" cx="0" cy="0" r="1" gradientTransform="matrix(16.5 12 -16.5 9.51398 16.5 12)" gradientUnits="userSpaceOnUse">
                            <stop stopColor="#7C8083"/>
                            <stop offset="1" stopColor="#6B7074"/>
                            </radialGradient>
                        </defs>


                        <SelectedBackground zoomLevel={zoomLevel} />

                        {activeFloorData.backgroundUrl && (
                            <image href={activeFloorData.backgroundUrl} x="0" y="0" width="540" height="900" />
                        )}

                        {activeFloorData.rooms.map(room => {
                            const colors = roomColors[room.type] || roomColors[RoomType.Other];

                            
                        return (
                            <React.Fragment key={room.roomId}>
                                
                                <path 
                                d={room.svgOutline}
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

                                {zoomLevel > 2 && room.interiorImageUrl && (
                                <image 
                                    href={room.interiorImageUrl} 
                                    x={room.interiorX} 
                                    y={room.interiorY} 
                                    width={room.interiorWidth} 
                                    height={room.interiorHeight}
                                    style={{ pointerEvents: 'none' }}
                                />
                                )}
                                    
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
                                                    <Icon icon={room.icon} style={{ width: '100%', height: '100%',
                                                    color: room.type === 3 ? '#647F97' : 
                                                            room.type === 4 ? '#89482A' : '#25292c' }} />
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

                        {activeFloorData.detailUrl && (
                            <image href={activeFloorData.detailUrl} x="0" y="0" width="540" height="900" />
                        )}
                    </svg>
                </TransformComponent>
            </TransformWrapper>
        </div>
    );
};

export default InteractiveMap;