import { Dispatch, SetStateAction } from 'react';
import { MatchCardInterface } from './TabSelection';
import Card from "@mui/material/Card";
import { useNavigate } from 'react-router-dom';
interface MatchCardProps {
    id: string
    name: string,
    imageData: string, 
    imageFormat: string,
    description: string,
    isVaccinated: boolean,
    isSterilized: boolean,
    age: number,
    createdAt: string
    cards: MatchCardInterface[],
    setCards: Dispatch<SetStateAction<MatchCardInterface[]>>
    handleMatchSelect : (id: string) => void
}

export default function MatchCard(props: MatchCardProps) {
    const navigate = useNavigate();

    function handleClick() {
        const {id, name, imageData, imageFormat, description, age, isVaccinated, isSterilized, createdAt} = props
        navigate(`/app/messages/${props.id}`, { state: { matchData: { id, name, imageData, imageFormat, description, age, isVaccinated, isSterilized, createdAt} } });
    }
    
    return(
        <Card
            sx={{
                width: "100%", 
                aspectRatio: "1 / 1",
                background: "none",
                border: "none",
                borderRadius: '5px',
                cursor: 'pointer',
                transition: 'transform 0.2s',
                '&:hover': {
                    transform: 'scale(1.1)'
                }
            }}
            onClick={handleClick}
        >
            <div className="match-img-wrapper">
                <img className='match-card' src={`data:${props.imageFormat};base64,${props.imageData}`} alt="Matched profile"/>
                <div className='match-info'>
                    <div className='match-info__general'>
                        <h1 className="match-name">{props.name}</h1>
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
    );
}