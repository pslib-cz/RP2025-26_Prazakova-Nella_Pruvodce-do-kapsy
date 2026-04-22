interface BackgroundProps { zoomLevel: number; }

const BackgroundM: React.FC<BackgroundProps> = ({ zoomLevel }) => (
    <g id="background-M">
        <path data-label="cesta" d="M135 530.5C134.164 604.745 133.213 786.006 135.22 792.5H100.096C100.932 718.762 102.103 539.887 100.096 512.286C98.089 484.685 112 446.5 118.5 433L135.22 286H156.5V332C157.336 368.7 146.063 447.417 156.5 460C169.546 475.728 247.692 481.531 258 480.5C268 479.5 303.813 470.513 323.883 468.653H357V492.499H331.912C312.176 495.543 274.811 502.139 283.24 504.168C291.67 506.198 319.2 527 331.912 537.147V570.634C315.688 553.383 244.968 516.461 184.5 501.5C136 489.5 135.502 506.823 135 530.5Z" fill="#DDDDDD" stroke="#BCBCBC"/>


        
        <g style={{ 
            opacity: zoomLevel > 1.5 ? 1 : 0, 
            transition: 'opacity 0.3s ease',
            visibility: zoomLevel > 1.5 ? 'visible' : 'hidden'
        }}>
            <circle cx="100" cy="200" r="10" fill="green" data-label="strom" />
        </g>
    </g>
);

export default BackgroundM;