import { Dispatch, SetStateAction } from 'react';
import { MatchCardinterface } from './TabSelection';
import Card from "@mui/material/Card";
import CardMedia from "@mui/material/CardMedia";

interface MatchCardProps {
    id: string
    imageData: string, 
    imageFormat: string,
    cards: MatchCardinterface[],
    setCards: Dispatch<SetStateAction<MatchCardinterface[]>>
    handleMatchSelect : (id: string) => void
}

export default function MatchCard(props: MatchCardProps) {
    return(
        <>
            <Card
                sx={{
                    width: "100%", 
                    aspectRatio: "1 / 1",
                    background: "none",
                    border: "none",
                    borderRadius: '5px'
                }}
            >
                <div className="match-img-wrapper">
                    <img className='match-card' src={`data:${props.imageFormat};base64,${props.imageData}`} alt="Matched profile"/>
                    <div className='match-info'>
                        <div className='match-info__general'>
                            <h1 className="match-name">Rex</h1>
                            <h1 className="match-age">12</h1>
                        </div>
                    </div>
                </div>
                {/* <CardMedia 
                    image={`data:${props.imageFormat};base64,${props.imageData}`}
                    src={`data:${props.imageFormat};base64,${props.imageData}`} 
                    className='match-card'
                    sx={{
                        objectFit: "cover",
                    }}
                    >
                </CardMedia> */}
            </Card>
        </>
    );
}