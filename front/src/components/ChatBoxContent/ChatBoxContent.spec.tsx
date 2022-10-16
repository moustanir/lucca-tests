import '@testing-library/jest-dom';
import { render, screen } from '@testing-library/react';
import ChatBoxContent from './ChatBoxContent';
import { ChatBoxesContext } from '../../contexts';

describe('ChatBoxContent tests suite', () => {
    it('should render the component', () => {
        // Given
        const authorName = 'authorName';

        // When
        render(<ChatBoxContent author={authorName} />);
        const chatBoxContainer = screen.getByTestId('chatBoxContent-container');

        // Then 
        expect(chatBoxContainer).toBeInTheDocument();
    });
});