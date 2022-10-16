import '@testing-library/jest-dom'
import {render, screen} from '@testing-library/react';
import ChatBox from './ChatBox';

describe('ChatBox tests suite', () =>{
    it('should render the component',() =>{
        // Given
        const authorName = 'authorName'; 
        const label = 'Chat A'; 
        
        // When
        render(<ChatBox label={label} author={authorName}/>);
        const chatBoxContainer = screen.getByTestId('chatbox-container');
        
        // Then 
        expect(chatBoxContainer).toBeInTheDocument();
    });

    it('should have the label',() =>{
        // Given
        const authorName = 'authorName'; 
        const label = 'Chat A'; 
        
        // When
        render(<ChatBox label={label} author={authorName}/>);
        const labelFound = screen.getByTestId('chatbox-label');
        
        // Then 
        expect(labelFound.innerHTML).toBe(label);
    });
})