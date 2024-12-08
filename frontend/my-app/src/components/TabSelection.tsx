import React, { useEffect, useState } from "react";
import Tabs from "@mui/material/Tabs"
import Tab from "@mui/material/Tab"
import Box from "@mui/material/Box"
import Typography from "@mui/material/Typography";
import MatchCard from "./MatchCard";
import { getMatches, MatchCardProfileData } from "../api/profileApi";
export type MatchCardInterface =  {
    id: string
    name: string,
    imageData: string,
    imageFormat: string
}
export default function TabSelection() {
    const [matchCards, setMatchCards] = useState<MatchCardInterface[]>([])
    const [activeTab, setActiveTab] = useState(0);
    useEffect(() => {
        document.body.style.background = '#FEFAF6';
        
        // api request to get all matches
        getMatches().then((data) => {
            const transformedCards = data.map(card => {
                return {
                    id: card.profile.id,
                    name: card.profile.name,
                    imageData : card.images[0].imageData,
                    imageFormat: card.images[0].imageFormat
                }
            })

            setMatchCards(transformedCards);
        })
        .catch(error => {
            console.error(error)
        });
        // setMatchCards(cards)
    }, [])
    const handleTabChange = (event : React.SyntheticEvent, value : number) => {
        setActiveTab(value);
    };

    const handleMatchSelect = (id : string) => {
        console.log("match card selected");
    }
    return(
        <>
        <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
            <Tabs 
                value={activeTab} 
                onChange={handleTabChange} 
                aria-label="basic tabs example" 
                sx={{
                    '& .MuiTabs-indicator': {
                        display: 'flex',
                        justifyContent: 'center',
                        backgroundColor: '#FEFAF6',
                    }}} >
                <Tab
                    sx={{
                        color:"#000",
                        '&.Mui-selected': {
                            color: '#FEFAF6'
                        }
                    }} 
                    label="Matches"/>
                <Tab 
                    sx={{
                        color:"#000", 
                        '&.Mui-selected': {
                            color: '#FEFAF6'
                        }
                    }} 
                    label="Messages"/>
            </Tabs>
        </Box>

        <Box sx={{ p: 2 }}>
                {activeTab === 0 && (
                    <Typography>
                        {matchCards && <div className="matches">
                            {matchCards.map((card) => {
                                return <MatchCard key={card.id} {...card} cards={matchCards} setCards={setMatchCards} handleMatchSelect={handleMatchSelect}/>
                            })}
                        </div>}
                    </Typography>
                )}
                {activeTab === 1 && (
                    <Typography>
                        Here are your messages!
                    </Typography>
                )}
            </Box>
        </>
    );
}

