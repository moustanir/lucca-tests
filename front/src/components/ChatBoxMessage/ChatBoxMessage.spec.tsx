import '@testing-library/jest-dom';
import {render, screen} from '@testing-library/react';
import ChatBoxMessage from './ChatBoxMessage';

describe('ChatBoxMessage tests suite', () =>{
    it('should render the component',() =>{
        // Given
        const authorName = 'authorName'; 
        const message = {content: 'test', author:'toto'};
        const id = 1;

        // When
        render(<ChatBoxMessage id={id} message={message} reader={authorName}/>);
        const chatBoxContainer = screen.getByTestId('chatBoxMessage-container');
        
        // Then 
        expect(chatBoxContainer).toBeInTheDocument();
    });

    it('should render an div with content',() =>{
        // Given
        const authorName = 'authorName'; 
        const message = {content: 'test', author:'toto'};
        const id = 1;

        // When
        render(<ChatBoxMessage id={id} message={message} reader={authorName}/>);
        const authorRendered = screen.getByTestId('chatBoxMessage-author') as HTMLDivElement;
        const messageRendered = screen.getByTestId('chatBoxMessage-content') as HTMLDivElement;
        
        // Then 
        expect(authorRendered).toBeInTheDocument();
        expect(authorRendered.innerHTML).toContain(message.author);
        expect(messageRendered).toBeInTheDocument();
        expect(messageRendered.innerHTML).toContain(message.content);
    });
})